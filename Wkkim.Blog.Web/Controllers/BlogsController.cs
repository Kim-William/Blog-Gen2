
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Wkkim.Blog.Web.Repositories;

namespace Wkkim.Blog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            var cont = blogPost.Content;

            var title_start = Regex.Matches(cont, "<p>coll-title-start</p>").Count;
            var title_end_cnt = Regex.Matches(cont, "<p>coll-title-end</p>").Count;
            var cont_end_cnt = Regex.Matches(cont, "<p>coll-cont-end</p>").Count;

            if (title_start-title_end_cnt == 0 && title_start - cont_end_cnt == 0)
            {
                cont = cont.Replace("<p>coll-title-start</p>", "<button type=\"button\" class=\"collapsible\" style=\"width:100%;text-align: left;\">");
                cont = cont.Replace("<p>coll-title-end</p>", "</button>\r\n    <div class=\"content\" style=\"display:none\">");
                cont = cont.Replace("<p>coll-cont-end</p>", "</div>");

                cont = cont.Replace("class=\"fr-text-bordered fr-text-uppercase\"", "class=\"fr-text-bordered fr-text-uppercase\" style=\"margin:0px\"");
            }

            if (!User.IsInRole("Admin"))
            {
                blogPost.Views++;
                var asd = await blogPostRepository.UpdateAsync(blogPost);
            }

            blogPost.Content = cont;
            return View(blogPost);
        }
    }
}

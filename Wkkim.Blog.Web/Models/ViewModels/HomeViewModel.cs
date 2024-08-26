
using Wkkim.Blog.Web.Models.Domain;

namespace Wkkim.Blog.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<BlogPost> BlogPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Wkkim.Blog.Web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

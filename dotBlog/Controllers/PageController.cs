using System.Linq;
using System.Web.Mvc;
using DotBlog.Models;

namespace DotBlog.Controllers
{
  public class PageController : Controller
  {
    PostContext postContext = new PostContext();

    public ActionResult Index(int pageNumber)
    {
      var posts = this.postContext.Posts.OrderBy(p => p.PostId).Skip((pageNumber - 1) * 3).Take(3);
      return View(posts);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotBlog.Models;

namespace DotBlog.Controllers
{
  public class HomeController : Controller
  {
    PostContext postContext = new PostContext();

    public ActionResult Index()
    {
      var posts = this.postContext.Posts.OrderBy(p => p.PostId).Take(3);
      return View("Index", posts);
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your app description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotBlog.Models;

namespace DotBlog.Controllers
{
  public class PostController : Controller
  {
    PostContext postContext = new PostContext();

    public ActionResult Index(string postUrl)
    {
      var post = this.postContext.Posts.Include("PostCategory").Include("PostTags").SingleOrDefault(p => p.Url == postUrl);
      if (post != null)
        return View(post);
      else 
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");      
    }
  }
}

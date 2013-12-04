using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotBlog
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
          name: "Default",
          url: "",
          defaults: new { controller = "Home", action = "Index" }
      );
      routes.MapRoute(
          name: "Post",
          url: "{postUrl}",
          defaults: new { controller = "Post", action = "Index" }
      );
      routes.MapRoute(
          name: "Page",
          url: "page/{pageNumber}",
          defaults: new { controller = "Page", action = "Index" }
      ); 
      routes.MapRoute(
        name: "AdminPosts",
        url: "admin/{action}",
        defaults: new { controller = "Admin", action = "Posts" }
      );
      routes.MapRoute(
        name: "AdminEditPost",
        url: "admin/edit/{postId}",
        defaults: new { controller = "Admin", action = "EditPost" }
      );
   


    }
  }
}
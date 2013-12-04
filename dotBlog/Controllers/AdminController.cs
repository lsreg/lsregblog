using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using DotBlog.Models;

namespace DotBlog.Controllers
{
  public class AdminController : Controller
  {
    PostContext postContext = new PostContext();

    public ActionResult Posts()
    {
      return View(this.postContext.Posts);
    }

    private string GetRssFeedUrl(string blog, int page)
    {
      return string.Format("http://{0}/feed/?paged={1}", blog, page);
    }

    public ActionResult ImportFromWordpressRss()
    {
      var blog = Request.QueryString["blog"];
      var page = 1;
      bool needContinue = true;
      while (needContinue)
      {
        needContinue = false;
        SyndicationFeed feed;
        using (var reader = XmlReader.Create(this.GetRssFeedUrl(blog, page)))
        {
          try
          {

            feed = SyndicationFeed.Load(reader);
          }
          catch (WebException ex)
          {
            // TODO: log me
            break;
          }
        }
        if (feed == null)
          break;

        foreach (SyndicationItem item in feed.Items)
        {
          needContinue = true;
          var post = new Post();
          post.Title = item.Title.Text;
          var uri = item.Links.First().Uri.ToString();
          post.Url = uri.Substring(uri.IndexOf(blog) + blog.Length).Replace("/", string.Empty);
          post.Body = item.ElementExtensions.ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/").First();
          post.State = PostState.Published;
          post.Description = item.Summary.Text;
          post.PublicationDate = item.PublishDate.DateTime;

          var wpcategory = item.Categories.FirstOrDefault();
          if (wpcategory != null)
          {
            var postCategory = this.postContext.Categories.FirstOrDefault(c => c.Name == wpcategory.Name);
            if (postCategory == null)
            {
              postCategory = new Category() { Name = wpcategory.Name };
              this.postContext.Categories.Add(postCategory);
            }
            post.PostCategory = postCategory;
            foreach (var wptag in item.Categories.Skip(1))
            {
              var postTag = this.postContext.Tags.FirstOrDefault(t => t.Name == wptag.Name);
              if (postTag == null)
              {
                postTag = new Tag() { Name = wptag.Name };
                this.postContext.Tags.Add(postTag);
              }
              post.PostTags.Add(postTag);
            }
          }

          this.postContext.Posts.Add(post);
          this.postContext.SaveChanges();
        }
        page++;
      }
      return Redirect("/admin/posts");
    }

    [HttpGet]
    public ActionResult EditPost(int postId)
    {
      return View(this.postContext.Posts.FirstOrDefault(p => p.PostId == postId));
    }

    [HttpPost]
    public ActionResult EditPost(Post post)
    {
      var pst = postContext.Posts.Single(p => p.PostId == post.PostId);
      postContext.Entry(pst).CurrentValues.SetValues(post);
      this.postContext.SaveChanges();
      return Redirect("/admin/posts");
    }

    [HttpGet]
    public ActionResult CreatePost()
    {
      var post = new Post();
      return View(post);
    }

    [HttpPost, ValidateInput(false)]
    public ActionResult CreatePost(Post post)
    {
      post.State = PostState.Draft;
      post.PublicationDate = DateTime.Now;
      this.postContext.Posts.Add(post);
      this.postContext.SaveChanges();
      return Redirect("/admin/posts");
    }

    [HttpGet]
    public ActionResult Categories()
    {
      var model = new CategoryModel() { Categories = this.postContext.Categories.ToList(), NewCategory = new Category() };
      return View(model);
    }

    [HttpPost]
    public ActionResult Categories(CategoryModel model)
    {
      this.postContext.Categories.Add(model.NewCategory);
      this.postContext.SaveChanges();
      return Redirect("/admin/categories");
    }



  }


}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotBlog.Models
{
  public class PostContext : DbContext
  {
    public PostContext() : base("DefaultConnection") { }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Post>().
        HasMany(p => p.PostTags).
        WithMany(t => t.Posts).
        Map(m =>
          {
            m.MapLeftKey("PostId");
            m.MapRightKey("TagId");
            m.ToTable("PostTagLinks");
          }
        );
    }
  }

  public enum PostState
  {
    Draft,
    Published,
    Deleted
  }

  [Table("Category")]
  public class Category
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

    public string Description { get; set; }
  }

  [Table("Tag")]
  public class Tag
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int TagId { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

    public ICollection<Post> Posts { get; set; }

    public Tag()
    {
      this.Posts = new List<Post>();
    }
  }

  public class CategoryModel
  {
    public IEnumerable<Category> Categories { get; set; }
    public Category NewCategory { get; set; }
  }
  
  [Table("Post")]
  public class Post
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int PostId { get; set; }
    public string Title { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    [AllowHtml]
    [DataType(DataType.MultilineText)]
    public string Body { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    [AllowHtml]
    public string Description { get; set; }

    public string Url { get; set; }

    public PostState State { get; set; }

    public DateTime? PublicationDate { get; set; }

    public Category PostCategory { get; set; }

    public ICollection<Tag> PostTags { get; set; }

    public Post()
    {
      this.PostTags = new List<Tag>();
    }
  }

}
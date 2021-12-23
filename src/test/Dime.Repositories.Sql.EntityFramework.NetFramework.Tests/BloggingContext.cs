using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    public class BloggingContext : DbContext
    {
        public BloggingContext()
        {
        }

        public BloggingContext(DbConnection connection) : base(connection, true)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.Id);

            modelBuilder.Entity<Blog>().HasKey(c => c.BlogId);
            modelBuilder.Entity<Blog>().HasOptional(c => c.Category);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public Category Category { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
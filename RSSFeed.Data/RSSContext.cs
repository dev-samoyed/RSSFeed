using Microsoft.EntityFrameworkCore;
using RSSFeed.Data.Entities;
using System;

namespace RSSFeed.Data
{
    public class RSSContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Post> Posts { get; set; }

        public RSSContext(DbContextOptions<RSSContext> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Channel>().HasData(
                new Channel
                {
                    Id = Guid.NewGuid(),
                    Title = "Interfax",
                    Url = "http://www.interfax.by/news/feed"
                },
                new Channel
                {
                    Id = Guid.NewGuid(),
                    Title = "Habr",
                    Url = "http://habrahabr.ru/rss/"
                }
            );
        }
    }
}

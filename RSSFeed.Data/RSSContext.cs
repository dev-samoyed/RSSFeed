using Microsoft.EntityFrameworkCore;
using RSSFeed.Data.Entities;
using System;

namespace RSSFeed.Data
{
    public class RSSContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public RSSContext(DbContextOptions<RSSContext> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasIndex(c => new { c.Name, c.ChannelId }).IsUnique();

            builder.Entity<Post>()
                .HasIndex(p => new { p.Title, p.ChannelId }).IsUnique();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace MusicForum.Data
{
    public class RPMForumContext : IdentityDbContext<ApplicationUser>
    {
        public RPMForumContext(DbContextOptions<RPMForumContext> options)
            : base(options)
        {
        }

        public DbSet<MusicForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<MusicForum.Models.Comment> Comment { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Discussion)
                .WithMany(d => d.Comments)
                .HasForeignKey(c => c.DiscussionId)
                .OnDelete(DeleteBehavior.Cascade); // If a discussion is deleted, delete its comments

            // Configure Comment and ApplicationUser relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ApplicationUser)
                .WithMany()
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull); // When a user is deleted, set ApplicationUserId to null

            
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicForum.Models;

namespace MusicForum.Data
{
    public class MusicForumContext : DbContext
    {
        public MusicForumContext (DbContextOptions<MusicForumContext> options)
            : base(options)
        {
        }

        public DbSet<MusicForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<MusicForum.Models.Comment> Comment { get; set; } = default!;
    }
}

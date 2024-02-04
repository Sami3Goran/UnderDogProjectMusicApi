using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UnderDogProjectMusicApi.Models;

namespace UnderDogProjectMusicApi.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}

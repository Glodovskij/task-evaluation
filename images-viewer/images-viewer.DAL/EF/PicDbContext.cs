using images_viewer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace images_viewer.DAL.EF
{
    public class PicDbContext : DbContext
    {
        public PicDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Pictures.db");
        }

        public DbSet<Picture> Pictures { get; set; }
    }
}

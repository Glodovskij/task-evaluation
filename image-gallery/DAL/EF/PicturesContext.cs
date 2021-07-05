using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class PicturesContext : DbContext
    {
        public PicturesContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Picture> Pictures { get; set; }
    }
}

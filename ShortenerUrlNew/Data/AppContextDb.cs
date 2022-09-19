using Microsoft.EntityFrameworkCore;
using ShortenerUrlNew.Models;

namespace ShortenerUrlNew.Data
{
    public class AppContextDb : DbContext
    {
        public DbSet<Url> urlList { get; set; }

        public AppContextDb(DbContextOptions<AppContextDb> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

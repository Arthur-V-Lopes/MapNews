using Microsoft.EntityFrameworkCore;
using NewsGlobe.Models;

namespace NewsGlobe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NewsItem> News => Set<NewsItem>();
    }
}

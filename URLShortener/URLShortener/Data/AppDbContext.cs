using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}

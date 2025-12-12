using Microsoft.EntityFrameworkCore;
using squares_api.Domain.Entities;

namespace squares_api.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Point> Points { get; set; }
    }
}

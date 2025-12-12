using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using squares_api.Domain.Entities;

namespace squares_api.Infrastructure.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                context.Database.EnsureCreated();
                // Look for any Student.
                if (context.Points.Any())
                {
                    return; // DB has been seeded
                }
                context.Points.AddRange(
                    new Point(0, 0),
                    new Point(1, 1),
                    new Point(1, 0),
                    new Point(0, 1)
                );
                context.SaveChanges();
            }
        }
    }
}

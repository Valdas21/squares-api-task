using squares_api.Domain.Interfaces;
using squares_api.Domain.Entities;
using squares_api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace squares_api.Infrastructure.Repositories
{
    public class PointRepo : IPointRepo
    {
        private readonly AppDbContext _context;

        public PointRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Point?> GetByIdAsync(int id)
        {
            return await _context.Points.FindAsync(id);
        }

        public async Task AddAsync(Point point)
        {
            await _context.Points.AddAsync(point);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Point> points)
        {
            await _context.Points.AddRangeAsync(points);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Point point)
        {
            _context.Points.Remove(point);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Point>> AllAsync()
        {
            return await _context.Points.ToListAsync();
        }
        public async Task<Point> GetByCoordinatesAsync(int x, int y)
        {
            return await _context.Points.FirstOrDefaultAsync(p => p.X == x && p.Y == y);
        }
        public async Task<bool> IsPresentPoint(int x, int y)
        {
            return await _context.Points.AnyAsync(p => p.X == x && p.Y == y);
        }
    }
}

using squares_api.Domain.Entities;

namespace squares_api.Domain.Interfaces
{
    public interface IPointRepo
    {
        Task<Point?> GetByIdAsync(int id);
        Task<Point> GetByCoordinatesAsync(int x, int y);
        Task<bool> IsPresentPoint(int x, int y);
        Task AddAsync(Point point);
        Task AddRangeAsync(IEnumerable<Point> points);
        Task DeleteAsync(Point point);
        Task<IEnumerable<Point>> AllAsync();
    }
}

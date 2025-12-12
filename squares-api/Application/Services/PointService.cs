using squares_api.Domain.Entities;
using squares_api.Domain.Interfaces;

namespace squares_api.Application.Services
{
    public class PointService
    {
        //todo prevent duplicates points from adding
        private readonly IPointRepo _pointRepo;

        public PointService(IPointRepo pointRepo)
        {
            _pointRepo = pointRepo;
        }

        public async Task<Point?> GetPointByIdAsync(int id)
        {
            return await _pointRepo.GetByIdAsync(id);
        }

        public async Task<bool> AddPointAsync(Point point)
        {
            if (await _pointRepo.IsPresentPoint(point.X, point.Y))
            {
                return false;
            }
            await _pointRepo.AddAsync(point);
            return true;
        }

        public async Task<bool> AddPointsAsync(IEnumerable<Point> points)
        {
            bool found = false;
            foreach (var point in points)
            {
                if (await _pointRepo.IsPresentPoint(point.X, point.Y))
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                await _pointRepo.AddRangeAsync(points);
            }
            return !found;
        }

        public async Task<bool> DeletePointAsync(int id)
        {
            var existingPoint = await _pointRepo.GetByIdAsync(id);
            if (existingPoint == null)
            {
                return false;
            }
            await _pointRepo.DeleteAsync(existingPoint);
            return true;
        }
        public async Task<IEnumerable<Square>?> RetrieveSquares()
        {
            var points = (await _pointRepo.AllAsync()).ToList();
            if (points == null || points.Count == 0)
            {
                return null;
            }
            
            Dictionary<string, Square> squares = new Dictionary<string, Square>();
            foreach (var A in points) 
            {
                foreach (var B in points)
                {
                    if (A == B) continue;
                    Point C = Point.Rotate(A, B);
                    Point firstFound = await _pointRepo.GetByCoordinatesAsync(C.X, C.Y);
                    if (firstFound == null) continue;

                    Point D = Point.Rotate(B, C);
                    Point secondFound = await _pointRepo.GetByCoordinatesAsync(D.X, D.Y);
                    if (secondFound == null) continue;
                    
                    var squarePoints = new[] { A, B, firstFound, secondFound }
                    .Select(p =>p.Id)
                    .OrderBy(v => v)
                    .ToArray();

                    string key = string.Join("|", squarePoints);

                    if (!squares.ContainsKey(key))
                    {
                        squares[key] = new Square(A, B, firstFound, secondFound);
                    }
                }
            }
            return squares.Values;

        }
    }
}

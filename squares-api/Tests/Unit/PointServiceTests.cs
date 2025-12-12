using Xunit;
using Moq;
using squares_api.Domain.Interfaces;
using squares_api.Domain.Entities;
using squares_api.Application.Services;
using Microsoft.EntityFrameworkCore;
using squares_api.Infrastructure.Data;
using squares_api.Infrastructure.Repositories;

namespace squares_api.Tests.Unit
{
    public class PointServiceTests
    {
        [Fact]
        public async Task AddPointAsync_ShouldReturnFalse_WhenPointAlreadyExists()
        {
            // Arrange
            var mockRepo = new Mock<IPointRepo>();
            var pointService = new PointService(mockRepo.Object);
            var existingPoint = new Point(1, 1);

            mockRepo.Setup(repo => repo.IsPresentPoint(existingPoint.X, existingPoint.Y))
                    .ReturnsAsync(true);

            // Act
            var result = await pointService.AddPointAsync(existingPoint);

            // Assert
            Assert.False(result);
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Point>()), Times.Never);
        }
        [Fact]
        public async Task AddPointAsync_ShouldReturnTrue_WhenPointDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IPointRepo>();
            var pointService = new PointService(mockRepo.Object);
            var newPoint = new Point(2, 2);

            mockRepo.Setup(repo => repo.IsPresentPoint(newPoint.X, newPoint.Y))
                    .ReturnsAsync(false);

            // Act
            var result = await pointService.AddPointAsync(newPoint);

            // Assert
            Assert.True(result);
            mockRepo.Verify(repo => repo.AddAsync(newPoint), Times.Once);
        }
        [Fact]
        public async Task DeletePointAsync_ShouldReturnFalse_WhenPointDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IPointRepo>();
            var pointService = new PointService(mockRepo.Object);
            int nonExistentPointId = 999;

            mockRepo.Setup(repo => repo.GetByIdAsync(nonExistentPointId))
                    .ReturnsAsync((Point?)null);

            // Act
            var result = await pointService.DeletePointAsync(nonExistentPointId);

            // Assert
            Assert.False(result);
            mockRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Point>()), Times.Never);
        }
        [Fact]
        public async Task DeletePointAsync_ShouldReturnTrue_WhenPointExists()
        {
            // Arrange
            var mockRepo = new Mock<IPointRepo>();
            var pointService = new PointService(mockRepo.Object);
            int existingPointId = 1;
            var existingPoint = new Point(1, 1) { Id = existingPointId };

            mockRepo.Setup(repo => repo.GetByIdAsync(existingPointId))
                    .ReturnsAsync(existingPoint);

            // Act
            var result = await pointService.DeletePointAsync(existingPointId);

            // Assert
            Assert.True(result);
            mockRepo.Verify(repo => repo.DeleteAsync(existingPoint), Times.Once);
        }
        [Fact]
        public async Task AddRangeAsync_ShouldReturnTrue_WhenNoPointsExist()
        {
            // Arrange
            var mockRepo = new Mock<IPointRepo>();
            var pointService = new PointService(mockRepo.Object);
            var newPoints = new List<Point>
            {
                new Point(3, 3),
                new Point(4, 4)
            };

            mockRepo.Setup(repo => repo.IsPresentPoint(It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(false);

            // Act
            var result = await pointService.AddPointsAsync(newPoints);

            // Assert
            Assert.True(result);
            mockRepo.Verify(repo => repo.AddRangeAsync(newPoints), Times.Once);
        }
        [Fact]
        public async Task AddRangeAsync_ShouldReturnFalse_WhenAtLeastOnePointExists()
        {
            // Arrange
            var mockRepo = new Mock<IPointRepo>();
            var pointService = new PointService(mockRepo.Object);
            var newPoints = new List<Point>
            {
                new Point(5, 5),
                new Point(1, 1)
            };

            mockRepo.SetupSequence(repo => repo.IsPresentPoint(It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(false)
                    .ReturnsAsync(true);

            // Act
            var result = await pointService.AddPointsAsync(newPoints);

            // Assert
            Assert.False(result);
            mockRepo.Verify(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<Point>>()), Times.Never);
        }
        [Fact]
        public async Task RetrieveSquares_ShouldReturnFiveSquares_WhenPointsFormThreeSquares()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            using var db = new AppDbContext(options);
            var repo = new PointRepo(db);
            var pointService = new PointService(repo);
            var points = new List<Point>
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 0),
                new Point(1, 1), // Square 1

                //new Point(1, 1),
                new Point(1, 2),
                new Point(2, 1),
                new Point(2, 2), // Square 2

                //new Point(2, 2),
                new Point(2, 3),
                new Point(3, 2),
                new Point(3, 3)  // Square 3

                //new Point(0,1),
                //new Point(1,0),
                //new Point(2,1),
                //new Point(1,2), // Square 4

                //new Point(2,1),
                //new Point(1,2),
                //new Point(3,2),
                //new Point(2,3), // Square 5
            };

            await pointService.AddPointsAsync(points);

            var squares = await pointService.RetrieveSquares();

            Assert.NotNull(squares);
            Assert.Equal(5, squares.Count());
        }
        [Fact]
        public async Task RetrieveSquares_ShouldReturnNull_WhenPointsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            using var db = new AppDbContext(options);
            var repo = new PointRepo(db);
            var pointService = new PointService(repo);

            var points = new List<Point>
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 0),
                new Point(2, 0)
            };

            // Act
            await pointService.AddPointsAsync(points);
            var result = await pointService.RetrieveSquares();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public async Task RetrieveSquares_ShouldReturnNull_WhenNoPointsExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            using var db = new AppDbContext(options);
            var repo = new PointRepo(db);
            var pointService = new PointService(repo);

            // Act
            var result = await pointService.RetrieveSquares();

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetPointByIdAsync_ShouldReturnPoint_WhenPointExists()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            using var db = new AppDbContext(options);
            var repo = new PointRepo(db);
            var pointService = new PointService(repo);
            int existingPointId = 1;
            var existingPoint = new Point(1, 1);

            await pointService.AddPointAsync(existingPoint);

            // Act
            var result = await pointService.GetPointByIdAsync(existingPointId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingPointId, result.Id);
        }
        [Fact]
        public async Task GetPointByIdAsync_ShouldReturnNull_WhenPointDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            using var db = new AppDbContext(options);
            var repo = new PointRepo(db);
            var pointService = new PointService(repo);
            int nonExistentPointId = 999;

            // Act
            var result = await pointService.GetPointByIdAsync(nonExistentPointId);

            // Assert
            Assert.Null(result);
        }
    }
}

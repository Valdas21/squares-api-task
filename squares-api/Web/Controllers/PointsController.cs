using Microsoft.AspNetCore.Mvc;
using squares_api.Application.Services;
using squares_api.Domain.Entities;

namespace squares_api.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly PointService _pointService;

        public PointsController(PointService pointService)
        {
            _pointService = pointService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Point>> GetPointById(int id)
        {
            var point = await _pointService.GetPointByIdAsync(id);
            if (point == null)
            {
                return NotFound("No point found");
            }
            return Ok(point);
        }
        [HttpPost]
        public async Task<ActionResult> AddPoint(Point point)
        {
            if(await _pointService.AddPointAsync(point))
            {
                return CreatedAtAction(nameof(GetPointById), new { id = point.Id }, point);
            }
            return Conflict("Point with the same coordinates already exists");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePoint(int id)
        {
            var deleted = await _pointService.DeletePointAsync(id);
            if (!deleted)
            {
                return NotFound("No point found to delete");
            }
            return NoContent();
        }
        [HttpPost("range")]
        public async Task<ActionResult> AddPointsRange(IEnumerable<Point> points)
        {
            if(points == null || !points.Any())
            {
                return BadRequest("Points collection is null or empty");
            }

            if(await _pointService.AddPointsAsync(points))
            {
                return Ok();
            }
            return Conflict("One or more points with the same coordinates already exist");

        }
        [HttpGet("squares")]
        public async Task<ActionResult<IEnumerable<Square>>> GetSquares()
        {
            var squares = await _pointService.RetrieveSquares();
            if (squares == null || !squares.Any())
            {
                return NotFound("No squares found");
            }
            return Ok(squares);
        }
    }
}

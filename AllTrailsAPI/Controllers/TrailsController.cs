using AllTrailsAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AllTrailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : ControllerBase
    {
        // Constructor
        public TrailsController()
        {
            // Initialize any dependencies or services here
        }

        // GET /api/trails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trail>>> GetAllTrails()
        {
            // Create a couple sample trails to return
            var trails = new List<Trail>
            {
                new Trail { Id = 1, Title = "Trail 1" },
                new Trail { Id = 2, Title = "Trail 2" }
            };

            return Ok(trails);
        }

    }
}

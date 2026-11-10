using AllTrailsAPI.DTOs;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Data;

namespace AllTrailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : ControllerBase
    {
        private readonly IDbConnection _db;

        // Constructor
        public TrailsController(IDbConnection db)
        {
            _db = db;
        }

        // GET /api/trails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trail>>> GetAllTrails()
        {
            var sql = @"SELECT t.*, c.*
                FROM Trail t 
                INNER JOIN Category c ON t.CategoryId = c.Id";

            // Fetch all trails from database. <Table 1, Table 2, Output>
            var trails = await _db.QueryAsync<Trail,Category,Trail>(
                sql,
                (trail, category) =>
                {
                    trail.Category = category;
                    return trail;
                });

            return Ok(trails);
        }

    }
}

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
                    // function is called for each row in the result set.
                    trail.Category = category;
                    return trail;
                },
                splitOn: "Id"); // indicates where to split the result set into Trail and Category objects. The first column of the next table.

            return Ok(trails);
        }

        // GET /api/trails/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Trail>>> GetTrailById(int id)
        {
            var sql = @"SELECT t.*, c.*, r.*, u.*
                FROM Trail t
                INNER JOIN Category c ON t.CategoryId = c.Id
                LEFT OUTER JOIN HikerReview r on t.Id = r.TrailId
                LEFT OUTER JOIN UserSnapshot u ON r.CreatedBy = u.UserId
                WHERE t.Id = @Id";

            // Create a null trail
            Trail? trail = null;

            // Fetch all trails from database. <Table 1, Table 2, Table 3, Table 4, Output>
            await _db.QueryAsync<Trail, Category, Review, User, Trail>(
                sql,
                (t, c, r, u) => // function is called for each row in the result set.
                {      
                    // populate trail object only for the first row in result set
                    if(trail == null)
                    {
                        trail = t;
                        trail.Category = c;
                        trail.Reviews = new List<Review>(); // initialize an empty list
                    }

                    if (r != null)
                    {
                        r.User = u;
                        trail.Reviews.Add(r);
                    }

                    return trail;
                },
                new { Id = id},
                splitOn: "Id,Id,UserId" // category ID, review ID, user ID
             ); // indicates where to split the result set into Trail and Category objects. The first column of the next table.

            return Ok(trail);
        }


            /*
             SELECT t.*, c.*, r.*, u.*
            FROM Trail t
            INNER JOIN Category c ON t.CategoryId = c.Id
            LEFT OUTER JOIN HikerReview r on t.Id = r.TrailId
            LEFT OUTER JOIN UserSnapshot u ON r.CreatedBy = u.UserId
            WHERE t.Id = 4002
            */

        }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParksApi.Models;

namespace ParksApi.Controllers.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class ParksController : ControllerBase
    {
        private readonly ParksApiContext _db;

        public ParksController(ParksApiContext db)
        {
        _db = db;
        }

        // GET api/v2/parks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Park>>> Get(string name, string state, string features, int rating, int? page)
        {
            var query = _db.Parks.AsQueryable();

            if (name != null)
            {
                query = query.Where(entry => entry.Name == name);
            }

            if (state != null)
            {
                query = query.Where(entry => entry.State == state);
            }

            if (features != null)
            {
                query = query.Where(entry => entry.Features == features);
            }

            if (rating != 0)
            {
                query = query.Where(entry => entry.Rating == rating);
            }
            int pageSize = 2;
            int currentPage = page ?? 1;

            var pagedReviews = PagedList<Park>.ToPagedList(query, currentPage, pageSize);

            return Ok(pagedReviews);

            
        }

        // GET: api/v2/parks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Park>> GetPark(int id)
        {
            Park park = await _db.Parks.FindAsync(id);

            if (park == null)
        {
            return NotFound();
        }

            return park;
        }

        // POST api/v2/parks
        [HttpPost]
        public async Task<ActionResult<Park>> Post(Park park)
        {
            _db.Parks.Add(park);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPark), new { id = park.ParkId }, park);
        }

        // PUT: api/v2/parks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Park park)
        
        {
            if (id != park.ParkId)
            {
                return BadRequest();
            }

            _db.Parks.Update(park);

            try
            {
                await _db.SaveChangesAsync();
            }
                catch (DbUpdateConcurrencyException)
            {
                if (!ParkExists(id))
                {
                    return NotFound();
                }
                    else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ParkExists(int id)
        {
            return _db.Parks.Any(e => e.ParkId == id);
        }

        // DELETE: api/v2/parks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePark(int id)
        {
            Park park = await _db.Parks.FindAsync(id);
            if (park == null)
        {
            return NotFound();
        }

            _db.Parks.Remove(park);
            await _db.SaveChangesAsync();

        return NoContent();
        }
    }
}
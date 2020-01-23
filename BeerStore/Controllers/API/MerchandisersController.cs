using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Merchandisers")]
    public class MerchandisersController : Controller
    {
        private readonly StoreContext _context;

        public MerchandisersController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Merchandisers
        [HttpGet]
        public IEnumerable<Merchandiser> GetMerchandisers()
        {
            return _context.Merchandisers;
        }

        // GET: api/Merchandisers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMerchandiser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var merchandiser = await _context.Merchandisers.SingleOrDefaultAsync(m => m.MerchandiserId == id);

            if (merchandiser == null)
            {
                return NotFound();
            }

            return Ok(merchandiser);
        }

        // PUT: api/Merchandisers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMerchandiser([FromRoute] int id, [FromBody] Merchandiser merchandiser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != merchandiser.MerchandiserId)
            {
                return BadRequest();
            }

            _context.Entry(merchandiser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MerchandiserExists(id))
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

        // POST: api/Merchandisers
        [HttpPost]
        public async Task<IActionResult> PostMerchandiser([FromBody] Merchandiser merchandiser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Merchandisers.Add(merchandiser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMerchandiser", new { id = merchandiser.MerchandiserId }, merchandiser);
        }

        // DELETE: api/Merchandisers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMerchandiser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var merchandiser = await _context.Merchandisers.SingleOrDefaultAsync(m => m.MerchandiserId == id);
            if (merchandiser == null)
            {
                return NotFound();
            }

            _context.Merchandisers.Remove(merchandiser);
            await _context.SaveChangesAsync();

            return Ok(merchandiser);
        }

        private bool MerchandiserExists(int id)
        {
            return _context.Merchandisers.Any(e => e.MerchandiserId == id);
        }
    }
}
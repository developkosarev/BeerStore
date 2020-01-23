using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/MarketingEvents")]
    public class MarketingEventsController : Controller
    {
        private readonly StoreContext _context;

        public MarketingEventsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/MarketingEvents
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public IEnumerable<MarketingEvent> GetMarketingEvent()
        {
            return _context.MarketingEvent;
        }

        // GET: api/MarketingEvents/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketingEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var marketingEvent = await _context.MarketingEvent.SingleOrDefaultAsync(m => m.MarketingEventId == id);

            if (marketingEvent == null)
            {
                return NotFound();
            }

            return Ok(marketingEvent);
        }

        // POST: api/MarketingEvents
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostMarketingEvent([FromBody] MarketingEvent marketingEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MarketingEvent MarketingEventdb = _context.MarketingEvent.FirstOrDefault(c => c.Code == marketingEvent.Code);
            if (MarketingEventdb != null)
            {
                MarketingEventdb.Descr = marketingEvent.Descr;
                MarketingEventdb.Version = marketingEvent.Version;
                MarketingEventdb.IsMark = marketingEvent.IsMark;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMarketingEvent", new { id = marketingEvent.MarketingEventId }, MarketingEventdb);
            }
            else
            {
                _context.MarketingEvent.Add(marketingEvent);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMarketingEvent", new { id = marketingEvent.MarketingEventId }, marketingEvent);
            }            
        }

        // DELETE: api/MarketingEvents/5
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarketingEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var marketingEvent = await _context.MarketingEvent.SingleOrDefaultAsync(m => m.MarketingEventId == id);
            if (marketingEvent == null)
            {
                return NotFound();
            }

            _context.MarketingEvent.Remove(marketingEvent);
            await _context.SaveChangesAsync();

            return Ok(marketingEvent);
        }

        private bool MarketingEventExists(int id)
        {
            return _context.MarketingEvent.Any(e => e.MarketingEventId == id);
        }
    }
}
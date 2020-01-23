using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Authorize]
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("api/MarketingEventProducts")]
    public class MarketingEventProductsController : Controller
    {
        private readonly StoreContext _context;

        public MarketingEventProductsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/MarketingEventProducts/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketingEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qry = _context.MarketingEventProduct.AsNoTracking().Where(c => c.ProductId == id)
                .Include(c => c.MarketingEvent);

            var items = await qry.ToListAsync();

            List<ViewMarketingEvent> viewMarketingEvents = new List<ViewMarketingEvent>();

            foreach (var item in items)
            {
                ViewMarketingEvent viewMarketingEvent =
                    new ViewMarketingEvent { Id = item.MarketingEventId, Descr = item.MarketingEvent.Descr };

                viewMarketingEvents.Add(viewMarketingEvent);
            }

            return Ok(viewMarketingEvents);
        }

        // GET: api/MarketingEventProducts/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public async Task<IActionResult> GetMarketingEventProduct([FromRoute] MarketingEventProductCode marketingEventProductCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MarketingEvent marketingEvent = _context.MarketingEvent.FirstOrDefault(p => p.Code == marketingEventProductCode.MarketingEventCode);
            Product products = _context.Products.FirstOrDefault(p => p.Code == marketingEventProductCode.ProductCode);

            var marketingEventProduct = await _context.MarketingEventProduct.SingleOrDefaultAsync(c => c.MarketingEventId == marketingEvent.MarketingEventId & c.ProductId == products.ProductId);

            if (marketingEventProduct == null)
            {
                return NotFound();
            }

            return Ok(marketingEventProduct);            
        }

        // POST: api/MarketingEventProducts
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostMarketingEventProduct([FromBody] List<MarketingEventProductCode> marketingEventProductCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int count = 0;
            foreach (var item in marketingEventProductCode)
            {
                MarketingEvent marketingEvent = _context.MarketingEvent.FirstOrDefault(p => p.Code == item.MarketingEventCode);
                Product product = _context.Products.FirstOrDefault(p => p.Code == item.ProductCode);

                if (marketingEvent == null)
                {
                    return BadRequest(ModelState);
                }

                if (product == null)
                {
                    return BadRequest(ModelState);
                }

                var marketingEventProduct = await _context.MarketingEventProduct.SingleOrDefaultAsync(c => c.MarketingEventId == marketingEvent.MarketingEventId & c.ProductId == product.ProductId);
                if (marketingEventProduct != null)
                {
                    marketingEventProduct.MarketingEvent = marketingEvent;
                    marketingEventProduct.Product = product;                    
                }
                else
                {
                    marketingEventProduct = new MarketingEventProduct();
                    marketingEventProduct.MarketingEvent = marketingEvent;
                    marketingEventProduct.Product = product;                    
                }

                _context.AttachRange(marketingEventProduct);
                count++;
            }

            await _context.SaveChangesAsync();

            return Ok(new { count = count });
        }

        // DELETE: api/MarketingEventProducts/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMarketingEventProduct([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var marketingEventProduct = await _context.MarketingEventProduct.SingleOrDefaultAsync(m => m.MarketingEventId == id);
        //    if (marketingEventProduct == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.MarketingEventProduct.Remove(marketingEventProduct);
        //    await _context.SaveChangesAsync();

        //    return Ok(marketingEventProduct);
        //}
        
    }
}

public class MarketingEventProductCode
{
    public string MarketingEventCode { get; set; }

    public string ProductCode { get; set; }
}

public class ViewMarketingEvent
{
    public int Id { get; set; }

    public string Descr { get; set; }
}
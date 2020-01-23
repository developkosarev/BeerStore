using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

using BeerStore.Helpers;
using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Authorize]
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("api/Outlets")]
    public class OutletsController : Controller
    {
        private readonly StoreContext _context;

        public OutletsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Outlets
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public PagesListResult<Outlet> GetOutlets(int customerId, string filter, int? pageNumber, int? pageSize)
        {
            var itemsPerPage = pageSize.HasValue ? pageSize.Value : 50;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;

            var qry = _context.Outlets.AsNoTracking().AsQueryable();
            if (customerId != 0)
            {
                qry = qry.Where(c => c.CustomerId == customerId);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(c => c.Descr.StartsWith(filter));
            }

            qry = qry.OrderBy(c => c.Descr);

            return PagesListResult<Outlet>.Create(qry, currentPageNum, itemsPerPage);
        }

        // GET: api/Outlets/5
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOutlet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var outlet = await _context.Outlets.SingleOrDefaultAsync(m => m.OutletId == id);

            if (outlet == null)
            {
                return NotFound();
            }

            return Ok(outlet);            
        }

        // POST: api/Outlets
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostOutlet([FromBody] Outlet outlet)        
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);                
            }

            Customer customer = _context.Customers.FirstOrDefault(p => p.Code == outlet.CustomerCode);

            Outlet outletdb = _context.Outlets.FirstOrDefault(p => p.Code == outlet.Code);
            if (outletdb != null)
            {                
                if (outletdb.Version != outlet.Version)
                {
                    outletdb.Descr = outlet.Descr;
                    outletdb.Kpp = outlet.Kpp;
                    outletdb.Version = outlet.Version;
                    outletdb.IsMark = outlet.IsMark;
                    if (customer != null) { 
                        outletdb.CustomerId = customer.CustomerId;
                    }
                
                    await _context.SaveChangesAsync();
                }

                return CreatedAtAction("GetOutlet", new { id = outletdb.OutletId }, outletdb);                
            }
            else
            {
                if (customer != null){
                    outlet.CustomerId = customer.CustomerId;
                }
                _context.Outlets.Add(outlet);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOutlet", new { id = outlet.OutletId }, outlet);                
            }
        }        

        private bool OutletExists(int id)
        {
            return _context.Outlets.Any(e => e.OutletId == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

using BeerStore.Helpers;
using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Authorize]
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("api/Warehouses")]
    public class WarehousesController : Controller
    {        
        private readonly StoreContext _context;

        public WarehousesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Warehouses
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public PagesListResult<Warehouse> GetWarehouses(string filter, int? pageNumber, int? pageSize, bool ShoppingArea = false)
        {
            var itemsPerPage = pageSize.HasValue ? pageSize.Value : 50;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;

            ApplicationUser user;
            int ShoppingAreaId = 0;

            if (ShoppingArea)
            {
                user = _context.Users.Single(x => x.UserName == HttpContext.User.Identity.Name);
                if (user != null)
                {
                    UserMeta userMeta = _context.UserMeta.FirstOrDefault(u => u.Id == user.UserMetaId);

                    ShoppingAreaId = (int)userMeta.ShoppingAreaId;
                }

                if (ShoppingAreaId == 0)
                {
                    ShoppingArea = false;
                };
            }

            var qry = _context.Warehouses.AsNoTracking().AsQueryable();

            if (ShoppingArea)
            {
                qry = _context.WarehouseShoppingArea.AsNoTracking()
                                .Where(c => c.ShoppingAreaId == ShoppingAreaId && c.UpdateStatus != 2)
                                .Include(c => c.Warehouse)
                                .Select(c => c.Warehouse);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(c => c.Descr.StartsWith(filter));
            }

            qry = qry.Where(c => c.IsMark == false);
            qry = qry.OrderBy(c => c.Descr);

            return PagesListResult<Warehouse>.Create(qry, currentPageNum, itemsPerPage);            
        }

        // GET: api/Warehouses/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var warehouse = await _context.Warehouses.SingleOrDefaultAsync(m => m.WarehouseId == id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return Ok(warehouse);
        }
        
        // POST: api/Warehouses
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostWarehouse([FromBody] Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Department department = _context.Departments.FirstOrDefault(p => p.Code == warehouse.DepartmentCode);

            Warehouse warehousedb = _context.Warehouses.FirstOrDefault(c => c.Code == warehouse.Code);
            if (warehousedb != null)
            {
                warehousedb.Descr = warehouse.Descr;
                warehousedb.Version = warehouse.Version;
                warehousedb.DepartmentCode = warehouse.DepartmentCode;
                //warehousedb.IsMark = warehouse.IsMark;
                if (department != null) {
                    warehousedb.DepartmentId = department.DepartmentId;
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetWarehouse", new { id = warehousedb.WarehouseId }, warehousedb);
            }
            else
            {
                if (department != null) {
                    warehouse.DepartmentId = department.DepartmentId;
                }
                _context.Warehouses.Add(warehouse);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetWarehouse", new { id = warehouse.WarehouseId }, warehouse);
            }            
        }
        
        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.WarehouseId == id);
        }
    }
}
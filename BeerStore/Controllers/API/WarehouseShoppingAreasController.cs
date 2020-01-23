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
    [Route("api/[controller]")]    
    public class WarehouseShoppingAreasController : ControllerBase
    {
        private readonly StoreContext _context;

        public WarehouseShoppingAreasController(StoreContext context)
        {
            _context = context;
        }

        //VUE
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpPost("addwarehouse")]
        public async Task<IActionResult> AddWarehouseShoppingArea([FromBody] Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleAsync(x => x.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "Sory, your user is empty !");

                return BadRequest(ModelState);
            }

            UserMeta userMeta = _context.UserMeta.FirstOrDefault(u => u.Id == user.UserMetaId);
            
            ShoppingArea shoppingArea = await _context.ShoppingArea.FirstOrDefaultAsync(x => x.ShoppingAreaId == userMeta.ShoppingAreaId);
            if (shoppingArea == null)
            {
                ModelState.AddModelError("", "Sory, your Shoping Area is empty !");

                return BadRequest(ModelState);
            }

            _context.AttachRange(warehouse);

            WarehouseShoppingArea warehouseShoppingArea = _context.WarehouseShoppingArea.FirstOrDefault(c => c.WarehouseId == warehouse.WarehouseId & c.ShoppingAreaId == shoppingArea.ShoppingAreaId);
            if (warehouseShoppingArea != null)
            {

                warehouseShoppingArea.ShoppingArea = shoppingArea;
                warehouseShoppingArea.Warehouse = warehouse;
                warehouseShoppingArea.UpdateStatus = 1;

                await _context.SaveChangesAsync();                
            }
            else
            {
                warehouseShoppingArea = new WarehouseShoppingArea();
                warehouseShoppingArea.ShoppingArea = shoppingArea;
                warehouseShoppingArea.Warehouse = warehouse;
                warehouseShoppingArea.UpdateStatus = 1;

                _context.WarehouseShoppingArea.Add(warehouseShoppingArea);
                await _context.SaveChangesAsync();                
            }

            return Ok();
        }

        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpPost("deletewarehouse")]
        public async Task<IActionResult> DeleteWarehouseShoppingArea([FromBody] Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleAsync(x => x.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "Sory, your user is empty !");

                return BadRequest(ModelState);
            }

            UserMeta userMeta = _context.UserMeta.FirstOrDefault(u => u.Id == user.UserMetaId);

            ShoppingArea shoppingArea = await _context.ShoppingArea.FirstOrDefaultAsync(x => x.ShoppingAreaId == userMeta.ShoppingAreaId);
            if (shoppingArea == null)
            {
                ModelState.AddModelError("", "Sory, your Shoping Area is empty !");

                return BadRequest(ModelState);
            }

            WarehouseShoppingArea warehouseShoppingArea = _context.WarehouseShoppingArea.FirstOrDefault(c => c.ShoppingAreaId == userMeta.ShoppingAreaId && c.WarehouseId == warehouse.WarehouseId);

            if (warehouseShoppingArea != null)
            {
                warehouseShoppingArea.UpdateStatus = 2;

                //_context.WarehouseShoppingArea.Remove(warehouseShoppingAreadb);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
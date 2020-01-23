using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    [Route("api/ShoppingAreas")]
    public class ShoppingAreasController : Controller
    {
        private readonly StoreContext _context;

        public ShoppingAreasController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingAreas
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public IEnumerable<ShoppingArea> GetShoppingArea()
        {
            return _context.ShoppingArea;
        }

        // GET: api/ShoppingAreas/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShoppingArea([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingArea = await _context.ShoppingArea.SingleOrDefaultAsync(m => m.ShoppingAreaId == id);

            if (shoppingArea == null)
            {
                return NotFound();
            }

            return Ok(shoppingArea);
        }

        // POST: api/ShoppingAreas
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostShoppingArea([FromBody] ShoppingArea shoppingArea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShoppingArea shoppingareadb = _context.ShoppingArea.FirstOrDefault(c => c.Code == shoppingArea.Code);
            if (shoppingareadb != null)
            {
                shoppingareadb.Descr = shoppingArea.Descr;
                shoppingareadb.Version = shoppingArea.Version;
                shoppingareadb.IsMark = shoppingArea.IsMark;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetShoppingArea", new {id = shoppingareadb.ShoppingAreaId}, shoppingareadb);
            }
            else
            {
                _context.ShoppingArea.Add(shoppingArea);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetShoppingArea", new {id = shoppingArea.ShoppingAreaId}, shoppingArea);
            }
        }

        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost("setuser")]
        public async Task<IActionResult> PostShoppingAreaUsers([FromBody] List<UserShoppingArea> userShoppingArea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int count = 0;
            foreach (var item in userShoppingArea)
            {              
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == item.UserId);                
                if (user != null)
                {
                    UserMeta userMeta = _context.UserMeta.FirstOrDefault(u => u.Id == user.UserMetaId);

                    ShoppingArea shoppingarea = await _context.ShoppingArea.FirstOrDefaultAsync(c => c.Code == item.ShoppingAreaCode);
                    if (shoppingarea != null)
                    {
                        userMeta.ShoppingAreaId = shoppingarea.ShoppingAreaId;                        
                        _context.AttachRange(userMeta);

                        //user.ShoppingAreaId = shoppingarea.ShoppingAreaId;
                        //_context.AttachRange(user);

                        count++;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { count = count });            
        }

        private bool ShoppingAreaExists(int id)
        {
            return _context.ShoppingArea.Any(e => e.ShoppingAreaId == id);
        }
    }

    public class UserShoppingArea
    {
        public string UserId { get; set; }

        public string ShoppingAreaCode { get; set; }
    }

}
using System;
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
    [Route("api/AgentPlusUsers")]
    public class AgentPlusUsersController : Controller
    {
        private readonly StoreContext _context;

        public AgentPlusUsersController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/AgentPlusUsers
        [HttpGet]
        public async Task<IActionResult>  GetAgentPlusUsers()
        {
            var qry = await _context.AgentPlusUsers
                .Include(m => m.Merchandiser)
                .Select(a => new
                {
                    Descr = a.Descr,
                    Code = a.Code
                })
                .ToListAsync();

            List<AgentPlusUser> agentPlusUsers = await _context.AgentPlusUsers
                .Include(m => m.Merchandiser)                
                .ToListAsync();
            
            return Ok(qry);            
        }
        
        //GET: api/AgentPlusUsers/v2
        [HttpGet("v2")]
        public async Task<IActionResult> GetAgentPlusUsersV2()
        {
            var qry = await _context.AgentPlusUsers
                .Include(m => m.Merchandiser)
                .Select(a => new
                {
                    Id = a.AgentPlusUserId,
                    Descr = a.Descr,
                    Code = a.Code,
                    MerchandiserCode = a.Merchandiser.Code
                })
                .ToListAsync();

            return Ok(qry);
        }

        //GET: api/AgentPlusUsers/v3
        [HttpGet("v3")]
        public async Task<IActionResult> GetAgentPlusUsersV3()
        {            
            var qry = await _context.AgentPlusUsers
                .Include(m => m.Merchandiser)
                .Include(m => m.AgentPlusUserShopingAreas).ThenInclude(s=> s.ShoppingArea)
                .Select(a => new
                    {
                        AgentPlusUserId = a.AgentPlusUserId,
                        Descr = a.Descr,
                        Code = a.Code,
                        ShopingAreas = a.AgentPlusUserShopingAreas.Select(b => new
                            {
                                ShoppingAreaId = b.ShoppingArea.ShoppingAreaId,
                                Descr = b.ShoppingArea.Descr,
                                Code = b.ShoppingArea.Code,
                        })
                    })
                .ToListAsync();
            
            return Ok(qry);
        }

        //GET: api/AgentPlusUsers/v4
        [HttpGet("v4")]
        public async Task<IActionResult> GetAgentPlusUsersV4()
        {            
            var qry = await _context.AgentPlusUsers
                .Include(m => m.Merchandiser)
                .Include(m => m.AgentPlusUserShopingAreas).ThenInclude(s => s.ShoppingArea)                
                .ToListAsync();
            
            return Ok(qry);
        }

        //GET: api/AgentPlusUsers/v5
        [HttpGet("v5")]
        public async Task<IActionResult> GetAgentPlusUsersV5()
        {                        
            var qry = await _context.AgentPlusUsers
                .Join(_context.AgentPlusUserShopingArea, (user => user.AgentPlusUserId), (area => area.AgentPlusUserId), (user, area) => new { user, area } )                
                .ToListAsync();

            return Ok(qry);
        }

        //GET: api/AgentPlusUsers/Update
        [HttpGet("Update")]
        public async Task<IActionResult> GetAgentPlusUsersUpdate()
        {
            var qry = await _context.AgentPlusUsers.Where(a => a.Update == true)
                .Select(a => new{ Code = a.Code, DateOrder = a.DateOrder})
                .ToListAsync();
            
            return Ok(qry);
        }

        //GET: api/AgentPlusUsers/id/blah
        [HttpGet("ID/{id}")]
        public async Task<IActionResult> GetAgentPlusUsers(string id)
        {
            var qry = await _context.AgentPlusUsers
                .Include(m => m.Merchandiser)
                .Select(a => new
                {
                    Id = a.AgentPlusUserId,
                    Descr = a.Descr,
                    Code = a.Code                    
                })
                .ToListAsync();
            
            return Ok(qry);            
        }


        // GET: api/AgentPlusUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgentPlusUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agentPlusUser = await _context.AgentPlusUsers.SingleOrDefaultAsync(m => m.AgentPlusUserId == id);

            if (agentPlusUser == null)
            {
                return NotFound();
            }

            return Ok(agentPlusUser);
        }
        
        // POST: api/AgentPlusUsers
        [HttpPost]
        public async Task<IActionResult> PostAgentPlusUser([FromBody] AgentPlusUser agentPlusUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AgentPlusUser AgentPlusUserdb = _context.AgentPlusUsers.FirstOrDefault(c => c.Code == agentPlusUser.Code);
            if (AgentPlusUserdb != null)
            {
                AgentPlusUserdb.Descr = agentPlusUser.Descr;
                AgentPlusUserdb.Version = agentPlusUser.Version;
                AgentPlusUserdb.IsMark = agentPlusUser.IsMark;                

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAgentPlusUser", new { id = agentPlusUser.AgentPlusUserId }, AgentPlusUserdb);
            }
            else
            {
                _context.AgentPlusUsers.Add(agentPlusUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAgentPlusUser", new { id = agentPlusUser.AgentPlusUserId }, agentPlusUser);
            }            
        }

        // POST: api/AgentPlusUsers/v2
        [HttpPost("v2")]
        public async Task<IActionResult> PostAgentPlusUserV2([FromBody] AgentPlusUser agentPlusUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AgentPlusUser agentPlusUserdb = _context.AgentPlusUsers.FirstOrDefault(c => c.Code == agentPlusUser.Code);
            if (agentPlusUserdb != null)
            {                
                agentPlusUserdb.Update = agentPlusUser.Update;
                //agentPlusUserdb.DateUpdate = agentPlusUser.DateUpdate;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAgentPlusUser", new { id = agentPlusUser.AgentPlusUserId }, agentPlusUserdb);
            }            

            return CreatedAtAction("GetAgentPlusUser", new { id = agentPlusUser.AgentPlusUserId }, agentPlusUser);
        }

        // POST: api/AgentPlusUsers/v3
        [HttpPost("v3")]
        public async Task<IActionResult> PostAgentPlusUserV3([FromBody] AgentPlusUser agentPlusUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AgentPlusUser agentPlusUserdb = _context.AgentPlusUsers.FirstOrDefault(c => c.Code == agentPlusUser.Code);
            if (agentPlusUserdb != null)
            {
                //agentPlusUserdb.Update = agentPlusUser.Update;
                agentPlusUserdb.DateUpdate = agentPlusUser.DateUpdate;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAgentPlusUser", new { id = agentPlusUser.AgentPlusUserId }, agentPlusUserdb);
            }

            return CreatedAtAction("GetAgentPlusUser", new { id = agentPlusUser.AgentPlusUserId }, agentPlusUser);
        }

        private bool AgentPlusUserExists(int id)
        {
            return _context.AgentPlusUsers.Any(e => e.AgentPlusUserId == id);
        }
    }
}
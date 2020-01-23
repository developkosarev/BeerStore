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
    [Route("api/[controller]")]    
    public class OrganisationsController : ControllerBase
    {
        private readonly StoreContext _context;

        public OrganisationsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Organisations
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public IEnumerable<Organisation> GetOrganisations()
        {
            return _context.Organisations;
        }

        // GET: api/Organisations/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganisation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organisation = await _context.Organisations.FindAsync(id);

            if (organisation == null)
            {
                return NotFound();
            }

            return Ok(organisation);
        }

        // PUT: api/Organisations/5
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganisation([FromRoute] int id, [FromBody] Organisation organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != organisation.OrganisationId)
            {
                return BadRequest();
            }

            _context.Entry(organisation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganisationExists(id))
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

        // POST: api/Organisations
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostOrganisation([FromBody] Organisation organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Organisation organisationdb = _context.Organisations.FirstOrDefault(p => p.Code == organisation.Code);
            if (organisationdb != null)
            {                
                organisationdb.Descr = organisation.Descr;                
                organisationdb.Inn = organisation.Inn;
                organisationdb.Version = organisation.Version;
                organisationdb.IsMark = organisation.IsMark;                

                await _context.SaveChangesAsync();                

                return CreatedAtAction("GetOrganisation", new { id = organisationdb.OrganisationId }, organisationdb);
            }
            
            _context.Organisations.Add(organisation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganisation", new { id = organisation.OrganisationId }, organisation);            
            
        }

        // DELETE: api/Organisations/5
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganisation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organisation = await _context.Organisations.FindAsync(id);
            if (organisation == null)
            {
                return NotFound();
            }

            _context.Organisations.Remove(organisation);
            await _context.SaveChangesAsync();

            return Ok(organisation);
        }

        private bool OrganisationExists(int id)
        {
            return _context.Organisations.Any(e => e.OrganisationId == id);
        }
    }
}
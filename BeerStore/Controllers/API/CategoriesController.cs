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
    [Route("api/Categories")]
    public class CategoriesController : Controller
    {
        private readonly StoreContext _context;

        public CategoriesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        //[AllowAnonymous]
        [HttpGet]
        public IEnumerable<Category> GetCategory()
        {
            return _context.Category;
        }

        // GET: api/Categories/5        
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/Categories
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category parent = _context.Category.FirstOrDefault(c => c.Code == category.ParentCategoryCode);
            if (parent != null)
            {
                category.ParentCategoryId = parent.CategoryId;
            }

            Category categorydb = _context.Category.FirstOrDefault(c => c.Code == category.Code);
            if (categorydb != null)
            {
                categorydb.Descr = category.Descr;
                categorydb.Version = category.Version;
                categorydb.IsMark = category.IsMark;
                categorydb.ParentCategoryCode = category.ParentCategoryCode;
                categorydb.ParentCategoryId = category.ParentCategoryId;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCategory", new { id = categorydb.CategoryId }, categorydb);
            }
            else
            {
                _context.Category.Add(category);

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
            }
        }
        
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}
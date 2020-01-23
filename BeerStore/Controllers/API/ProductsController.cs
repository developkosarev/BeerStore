using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }
        
        // GET: api/Products
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        //[AllowAnonymous]
        [HttpGet]
        public PagesListResult<Product> GetProducts(int categoryId, string filter, int? pageNumber, int? pageSize)
        {
            var itemsPerPage = pageSize.HasValue ? pageSize.Value : 50;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;

            var qry = _context.Products.AsNoTracking().AsQueryable();
            qry = qry.Where(c => c.CategoryId == categoryId);

            
            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(c => c.Descr.StartsWith(filter));
            }            

            qry = qry.OrderBy(c => c.Descr);            

            return PagesListResult<Product>.Create(qry, currentPageNum, itemsPerPage);
        }

        // GET: api/Products/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)        
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);                
            }

            Product productdb = _context.Products.FirstOrDefault(p => p.Code == product.Code);
            if (productdb != null)
            {
                if (productdb.Version != product.Version)
                {
                    productdb.Descr = product.Descr;
                    productdb.Version = product.Version;
                    productdb.IsMark = product.IsMark;
                    productdb.CategoryId = product.CategoryId;
                    productdb.Pack = product.Pack;
                    //productdb.Price = product.Price;

                    await _context.SaveChangesAsync();
                }
                
                return CreatedAtAction("GetProduct", new {id = productdb.ProductId }, productdb);
            }
            else
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduct", new {id = product.ProductId}, product);                
            }
        }

        // POST: api/Products/price
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost("price")]
        public async Task<IActionResult> PostProductPrice([FromBody] List<ProductPrice> productPrice)        
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);            
            }

            int count = 0;
            foreach (var item in productPrice)
            {
                Product productdb = _context.Products.FirstOrDefault(p => p.Code == item.Code);
                if (productdb != null){

                    productdb.Price = item.Price;
                    productdb.Price1 = item.Price1;
                    productdb.Quantity = item.Quantity;
                    productdb.Quantity1 = item.Quantity1;
                    _context.AttachRange(productdb);

                    count++;
                }                    
            }

            await _context.SaveChangesAsync();

            return Ok(new { count = count });
            
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }

    public class ProductPrice
    {
        public string Code { get; set; }

        public decimal Price { get; set; }

        public decimal Price1 { get; set; }

        public decimal Quantity { get; set; }

        public decimal Quantity1 { get; set; }
    }
}
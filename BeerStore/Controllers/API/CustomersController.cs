using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly StoreContext _context;

        public CustomersController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public PagesListResult<Customer> GetCustomers(string filter, int? pageNumber, int? pageSize, bool ShoppingArea = false)
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

                if (ShoppingAreaId == 0 ) {
                    ShoppingArea = false;
                };
            }


            var qry = _context.Customers.AsNoTracking().AsQueryable();

            if (ShoppingArea) {
                
                qry = _context.CustomerShoppingArea.AsNoTracking()
                                .Where(c => c.ShoppingAreaId == ShoppingAreaId && c.UpdateStatus != 2)
                                //.OrderBy(c => c.Customer.Descr)
                                .Include(c => c.Customer)
                                .Select(c => c.Customer);

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qry = qry.Where(c => c.Descr.StartsWith(filter));
                }

                qry = qry.OrderBy(c => c.Descr);

            } else {
                qry = _context.Customers.AsNoTracking().AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qry = qry.Where(c => c.Descr.StartsWith(filter));
                }

                qry = qry.OrderBy(c => c.Descr);
            }            

            return PagesListResult<Customer>.Create(qry, currentPageNum, itemsPerPage);
        }
        
        // GET: api/Customers/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customers
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customerdb = _context.Customers.FirstOrDefault(p => p.Code == customer.Code);
            if (customerdb != null)
            {
                //if (customerdb.Version != customer.Version)
                //{
                    customerdb.Descr = customer.Descr;
                    customerdb.Inn = customer.Inn;
                    customerdb.Version = customer.Version;
                    customerdb.IsMark = customer.IsMark;
                    customerdb.AddressPost = customer.AddressPost;

                await _context.SaveChangesAsync();
                //}

                return CreatedAtAction("GetCustomer", new { id = customerdb.CustomerId }, customerdb);
            }
            else
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
            }            
        }
        

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
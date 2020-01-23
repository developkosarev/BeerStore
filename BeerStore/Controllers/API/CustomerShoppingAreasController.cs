using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Authorize]
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("api/CustomerShoppingAreas")]
    public class CustomerShoppingAreasController : Controller
    {
        private readonly StoreContext _context;

        public CustomerShoppingAreasController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/CustomerShoppingAreas
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{updateStatus}")]        
        public async Task<IActionResult> GetCustomerShoppingArea([FromRoute] int updateStatus)
        {            
            var qry = _context.CustomerShoppingArea.AsNoTracking().Where(c=> c.UpdateStatus == updateStatus)
                .Include(c => c.Customer)
                .Include(c => c.ShoppingArea);

            var items = await qry.ToListAsync();

            List<CustomerShoppingAreaCode> customerShoppingAreaCodes = new List<CustomerShoppingAreaCode>();

            foreach (var item in items)
            {
                CustomerShoppingAreaCode customerShoppingAreaCode =
                    new CustomerShoppingAreaCode { ShoppingAreaCode = item.ShoppingArea.Code, CustomerCode = item.Customer.Code};

                customerShoppingAreaCodes.Add(customerShoppingAreaCode);
            }

            return Ok(customerShoppingAreaCodes);
        }

        // GET: api/CustomerShoppingAreas/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerShoppingArea([FromBody] CustomerShoppingAreaCode customerShoppingAreaCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = _context.Customers.FirstOrDefault(p => p.Code == customerShoppingAreaCode.CustomerCode);
            ShoppingArea shoppingArea = _context.ShoppingArea.FirstOrDefault(p => p.Code == customerShoppingAreaCode.ShoppingAreaCode);

            var customerShoppingArea = await _context.CustomerShoppingArea.SingleOrDefaultAsync(c => c.CustomerId == customer.CustomerId & c.ShoppingAreaId == shoppingArea.ShoppingAreaId);

            if (customerShoppingArea == null)
            {
                return NotFound();
            }            

            return Ok(customerShoppingArea);
        }

        // POST: api/CustomerShoppingAreas
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpPost]
        public async Task<IActionResult> PostCustomerShoppingArea([FromBody] CustomerShoppingAreaCode customerShoppingAreaCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }                        

            Customer customer = _context.Customers.FirstOrDefault(p => p.Code == customerShoppingAreaCode.CustomerCode);
            ShoppingArea shoppingArea = _context.ShoppingArea.FirstOrDefault(p => p.Code == customerShoppingAreaCode.ShoppingAreaCode);

            if (customer == null)
            {
                return BadRequest(ModelState);
            }

            if (shoppingArea == null)
            {
                return BadRequest(ModelState);
            }

            CustomerShoppingArea customerShoppingArea = _context.CustomerShoppingArea.FirstOrDefault(c => c.CustomerId == customer.CustomerId & c.ShoppingAreaId == shoppingArea.ShoppingAreaId);
            if (customerShoppingArea != null)
            {

                customerShoppingArea.ShoppingArea = shoppingArea;
                customerShoppingArea.Customer = customer;
                customerShoppingArea.UpdateStatus = 0;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomerShoppingArea", new CustomerShoppingAreaCode { ShoppingAreaCode = shoppingArea.Code, CustomerCode = customer.Code});

            }
            else
            {
                customerShoppingArea = new CustomerShoppingArea();
                customerShoppingArea.ShoppingArea = shoppingArea;
                customerShoppingArea.Customer = customer;
                customerShoppingArea.UpdateStatus = 0;

                _context.CustomerShoppingArea.Add(customerShoppingArea);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomerShoppingArea", new CustomerShoppingAreaCode { ShoppingAreaCode = shoppingArea.Code, CustomerCode = customer.Code });
            }

        }
        
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpPost("addcustomer")]
        public async Task<IActionResult> AddCustomerShoppingArea([FromBody] Customer customer)
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
            if (shoppingArea == null) {
                ModelState.AddModelError("", "Sory, your Shoping Area is empty !");

                return BadRequest(ModelState);
            }

            _context.AttachRange(customer);

            CustomerShoppingArea customerShoppingArea = _context.CustomerShoppingArea.FirstOrDefault(c => c.CustomerId == customer.CustomerId & c.ShoppingAreaId == shoppingArea.ShoppingAreaId);
            if (customerShoppingArea != null)
            {

                customerShoppingArea.ShoppingArea = shoppingArea;
                customerShoppingArea.Customer = customer;
                customerShoppingArea.UpdateStatus = 1;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomerShoppingArea", new CustomerShoppingAreaCode { ShoppingAreaCode = shoppingArea.Code, CustomerCode = customer.Code });

            }
            else
            {
                customerShoppingArea = new CustomerShoppingArea();
                customerShoppingArea.ShoppingArea = shoppingArea;
                customerShoppingArea.Customer = customer;
                customerShoppingArea.UpdateStatus = 1;

                _context.CustomerShoppingArea.Add(customerShoppingArea);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomerShoppingArea", new CustomerShoppingAreaCode { ShoppingAreaCode = shoppingArea.Code, CustomerCode = customer.Code });
            }

        }

        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpPost("deletecustomer")]
        public async Task<IActionResult> DeleteCustomerShoppingArea([FromBody] Customer customer)
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

            CustomerShoppingArea customerShoppingArea = _context.CustomerShoppingArea.FirstOrDefault(c => c.ShoppingAreaId == userMeta.ShoppingAreaId && c.CustomerId == customer.CustomerId);

            if (customerShoppingArea != null)
            {
                customerShoppingArea.UpdateStatus = 2;                
                await _context.SaveChangesAsync();
            }

            return Ok();
        }        
    }

    public class CustomerShoppingAreaCode
    {        
        public string ShoppingAreaCode { get; set; }

        public string CustomerCode { get; set; }
    }

}
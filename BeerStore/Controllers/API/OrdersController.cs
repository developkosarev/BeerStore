using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using BeerStore.Helpers;
using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Authorize]
    [EnableCors("AllowAllOrigin")]
    [Route("api/[controller]")]    
    public class OrdersController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OrdersController(StoreContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        // GET: api/Orders
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public PagesListResult<Order> GetOrders(string filter, int? pageNumber, int? pageSize)
        {
            var itemsPerPage = pageSize.HasValue ? pageSize.Value : 50;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;
                        
            string UserId = GetUserId();
            
            var qry = _context.Orders.AsNoTracking().AsQueryable();

            qry = qry.Where(c => c.UserId == UserId)
                .OrderByDescending(c => c.Date)
                .Include(o => o.Customer)
                .Include(o => o.Outlet)
                .Include(o => o.Warehouse);            

            return PagesListResult<Order>.Create(qry, currentPageNum, itemsPerPage);            
        }

        // GET: api/Orders/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        //[AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string UserId = GetUserId();
            
            var qry = _context.Orders.AsNoTracking().AsQueryable();

            qry = qry.Where(c => c.OrderId == id && c.UserId == UserId)
                .OrderByDescending(c => c.Date)
                .Include(o => o.Customer)
                .Include(o => o.Outlet)
                .Include(o => o.Warehouse)
                .Include(o => o.Lines).ThenInclude(l => l.Product)
                .Include(o => o.Lines).ThenInclude(l => l.MarketingEvent);

            var order = await qry.FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }            

            return Ok(order);            
        }
        
        // POST: api/Orders
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            if ( !string.IsNullOrWhiteSpace(order.Number) ) {
                Order orderdb = await _context.Orders.SingleOrDefaultAsync(m => m.Number == order.Number);

                if (orderdb != null) {
                    ModelState.AddModelError("", "Sory, order exist !");
                }
            }

            //Заполняем из БД
            if (order.Customer.CustomerId == 0)
            {
                Customer customer = await _context.Customers.SingleOrDefaultAsync(m => m.Code == order.Customer.Code);
                if (customer != null)
                {
                    order.Customer = customer;
                }
            }

            if (order.Outlet.OutletId == 0)
            {
                Outlet outlet = await _context.Outlets.SingleOrDefaultAsync(m => m.Code == order.Outlet.Code);
                if (outlet != null)
                {
                    order.Outlet = outlet;
                }
            }

            if (order.Warehouse.WarehouseId == 0)
            {
                Warehouse warehouse = await _context.Warehouses.SingleOrDefaultAsync(m => m.Code == order.Warehouse.Code);
                if (warehouse != null)
                {
                    order.Warehouse = warehouse;
                }
            }

            foreach (var item in order.Lines)
            {
                if (item.Product.ProductId == 0)
                {
                    Product product = await _context.Products.SingleOrDefaultAsync(m => m.Code == item.Product.Code);
                    if (product != null)
                    {
                        item.Product = product;
                    }
                }
            }


            if (order.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sory, your cart is empty !");
            }            

            if (order.Customer.CustomerId == 0)
            {
                ModelState.AddModelError("", "Sory, your customer is empty !");
            }

            if (order.Outlet.OutletId == 0)
            {
                ModelState.AddModelError("", "Sory, your outlet is empty !");
            }

            if (order.Warehouse.WarehouseId == 0)
            {
                ModelState.AddModelError("", "Sory, your warehouse is empty !");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.Shipped = false;
            order.Date = DateTime.Now;
            
            var userApi = await _context.Users.SingleAsync(x => x.UserName == HttpContext.User.Identity.Name);
            if (userApi != null) {
                order.UserId = userApi.Id;
                order.FirstName = userApi.FirstName;
                order.LastName = userApi.LastName;
            }            
            
            _context.AttachRange(order.Customer);
            _context.AttachRange(order.Outlet);
            _context.AttachRange(order.Warehouse);
            _context.AttachRange(order.Lines.Select(l => l.Product));
            
            var marketingEventList = order.Lines.Where(m => m.MarketingEvent != null).Select(l => l.MarketingEvent);

            List<int> newList = new List<int>();
            foreach (var item in marketingEventList)
            {
                if (newList.IndexOf(item.MarketingEventId) == -1)
                {
                    newList.Add(item.MarketingEventId);
                    _context.AttachRange(item);
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }



        // GET: api/Exchange        
        [HttpGet("GetNewOrder")]
        public IEnumerable<Order> GetOrderNew()
        {
            //Использование прямых запросов 2.1
            //http://programmerz.ru/questions/15381/raw-sql-query-without-dbset-entity-framework-core-question.html

            //var result = _context.OrderExchanges.FromSql("SELECT * From Orders").ToList();

            var qry = _context.Orders.AsNoTracking().Where(o => o.Shipped == false).OrderBy(o => o.OrderId)
                .Include(o => o.Customer)
                .Include(o => o.Outlet)
                .Include(o => o.Warehouse)
                .Include(o => o.Lines).ThenInclude(l => l.Product)
                .Include(o => o.Lines).ThenInclude(l => l.MarketingEvent);

            return qry;
        }

        // PUT: api/Exchange/5
        [HttpPut("{id}")]
        public void PutOrder(int id, [FromBody] string value)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order != null)
            {
                order.Shipped = true;
                //_context.SaveOrder(order);

                var marketingEventList = order.Lines.Where(m => m.MarketingEvent != null).Select(l => l.MarketingEvent);

                List<int> newList = new List<int>();
                foreach (var item in marketingEventList)
                {
                    if (newList.IndexOf(item.MarketingEventId) == -1)
                    {
                        newList.Add(item.MarketingEventId);
                        _context.Attach(item);
                    }
                }

                _context.AttachRange(order.Lines.Select(l => l.Product));
                _context.AttachRange(order.Customer);
                _context.AttachRange(order.Outlet);
                _context.AttachRange(order.Warehouse);
                if (order.OrderId == 0)
                {
                    _context.Orders.Add(order);
                }
                _context.SaveChanges();
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        private string GetUserId()
        {
            string UserId = "0";

            if (HttpContext.User.Identity == null)
            {
                return UserId;
            }

            var user = _context.Users.Single(x => x.UserName == HttpContext.User.Identity.Name);
            if (user != null)
            {
                UserId = user.Id;
            }

            return UserId;
        }
    }    
}
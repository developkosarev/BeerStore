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
    public class StocksController : ControllerBase
    {
        private readonly StoreContext _context;

        public StocksController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Stocks
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public async Task<IActionResult> GetStock(int productId, int warehouseId)
        {            
            var qry = _context.Stock.AsNoTracking().AsQueryable();
            qry = qry.Where(c => c.ProductId == productId && c.WarehouseId == warehouseId);
            
            List<Stock> stock = await qry.ToListAsync();            

            return Ok(stock);
        }
                
        // POST: api/Stocks
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostStock([FromBody] List<StockCode> stockCode)
        {            
            string warehouseCode = "";

            if (stockCode.Count() > 0)
            {
                warehouseCode = stockCode[0].WarehouseCode;
            }
            else
            {
                ModelState.AddModelError("", "Sory, your list is empty !");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockDiff = _context.Stock.Where(c => c.Warehouse.Code == warehouseCode)
                .Include(c => c.Product)
                .Include(c => c.Warehouse)
                .Select(c => new StockCodeDiff
                {
                    ProductCode = c.Product.Code,
                    WarehouseCode = c.Warehouse.Code,
                    QuantityOld = c.Quantity                    
                }).ToList();
            
            foreach (var item in stockCode)
            {
                stockDiff.Add(new StockCodeDiff
                      { ProductCode = item.ProductCode,
                        WarehouseCode = item.WarehouseCode,
                        QuantityOld = 0,
                        QuantityNew = item.Quantity });
            }

            var StockGroups = from itemStock in stockDiff
                              group itemStock by new { itemStock.ProductCode, itemStock.WarehouseCode } into g
                              select new { Resources = g.Key, QuantityOld = g.Sum(c => c.QuantityOld), QuantityNew = g.Sum(c => c.QuantityNew) };


            Warehouse warehouse = _context.Warehouses.FirstOrDefault(p => p.Code == warehouseCode);

            int count = 0;
            if (warehouse != null) {

                foreach (var group in StockGroups)
                {                    
                    if (group.QuantityOld == group.QuantityNew)
                    {
                        continue;
                    }

                    Product product = _context.Products.FirstOrDefault(p => p.Code == group.Resources.ProductCode);
                    if (product != null) {                    
                        Stock stock = _context.Stock.FirstOrDefault(c => c.ProductId == product.ProductId & c.WarehouseId == warehouse.WarehouseId);
                        if (stock != null)
                        {
                            stock.Quantity = group.QuantityNew;
                            
                            _context.AttachRange(stock);
                            count++;
                        }
                        else
                        {
                            stock = new Stock();
                            stock.Product = product;
                            stock.Warehouse = warehouse;
                            stock.Quantity = group.QuantityNew;

                            _context.AttachRange(stock);

                            count++;
                        }
                    }
                }
            }

            if (count > 0)
            {
                await _context.SaveChangesAsync();
            }            

            return Ok(new { count = count });
        }
        
        public class StockCode
        {
            public string ProductCode { get; set; }

            public string WarehouseCode { get; set; }

            public decimal Quantity { get; set; }

        }

        public class StockCodeDiff
        {
            public string ProductCode { get; set; }

            public string WarehouseCode { get; set; }

            public decimal QuantityOld { get; set; }

            public decimal QuantityNew { get; set; }

        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class Stock
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        [Column(TypeName = "numeric(20,3)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "numeric(20,3)")]
        public decimal ReservedQuantity { get; set; }

    }
}

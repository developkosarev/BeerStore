using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{    
    [Table("Warehouse")]
    public class Warehouse
    {
        [Column("WarehouseId")]
        public int WarehouseId { get; set; }

        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter a warehouse name")]
        public string Descr { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public List<WarehouseShoppingArea> WarehouseShoppingAreas { get; set; }

        public List<Stock> Stocks { get; set; }

        public int? DepartmentId { get; set; }

        public Department Department { get; set; }

        public string DepartmentCode { get; set; }

        public Warehouse()
        {
            WarehouseShoppingAreas = new List<WarehouseShoppingArea>();

            Stocks = new List<Stock>();
        }
    }
}

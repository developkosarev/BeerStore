using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class WarehouseShoppingArea
    {        
        public int ShoppingAreaId { get; set; }

        public ShoppingArea ShoppingArea { get; set; }        

        public int WarehouseId { get; set; }        

        public Warehouse Warehouse { get; set; }

        public int UpdateStatus { get; set; } //0-Без изменений, 1-Добавлен, инзменение, 2-Удален
    }
    
}

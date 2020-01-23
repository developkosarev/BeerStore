using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class CustomerShoppingArea
    {        
        public int ShoppingAreaId { get; set; }

        public ShoppingArea ShoppingArea { get; set; }        

        public int CustomerId { get; set; }        

        public Customer Customer { get; set; }

        public int UpdateStatus { get; set; } //0-Без изменений, 1-Добавлен, инзменение, 2-Удален
    }
    
}

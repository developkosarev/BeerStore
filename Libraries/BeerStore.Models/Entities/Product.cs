using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Please enter a product name")]
        public string Descr { get; set; }

        public string Code { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }        

        public int Pack { get; set; }

        [Column(TypeName = "numeric(15,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "numeric(15,2)")]
        public decimal Price1 { get; set; } //Voljsliy

        [Column(TypeName = "numeric(20,3)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "numeric(20,3)")]
        public decimal Quantity1 { get; set; } //Voljsliy

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public List<MarketingEventProduct> MarketingEventProducts { get; set; }

        public List<Stock> Stocks { get; set; }

        public Product()
        {
            MarketingEventProducts = new List<MarketingEventProduct>();

            Stocks = new List<Stock>();
        }

    }
}

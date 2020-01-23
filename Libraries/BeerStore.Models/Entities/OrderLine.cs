using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerStore.Models.Entities
{

    [Table("OrdersLine")]
    public class CartLine
    {
        public int CartLineId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public MarketingEvent MarketingEvent { get; set; }
    }

}

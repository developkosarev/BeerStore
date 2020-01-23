using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class MarketingEventProduct
    {
        public int MarketingEventId { get; set; }

        public MarketingEvent MarketingEvent { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}

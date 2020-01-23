using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class MarketingEvent
    {
        public int MarketingEventId { get; set; }
       
        public string Descr { get; set; }

        public string Code { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public List<MarketingEventProduct> MarketingEventProducts { get; set; }

        public MarketingEvent()
        {
            MarketingEventProducts = new List<MarketingEventProduct>();
        }

    }
}

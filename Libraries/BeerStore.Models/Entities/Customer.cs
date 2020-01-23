using System.Collections.Generic;

namespace BeerStore.Models.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Code { get; set; }

        public string Descr { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public string Inn { get; set; }

        public string AddressPost { get; set; }

        public ICollection<Outlet> Outlets { get; set; }

        public List<CustomerShoppingArea> CustomerShoppingAreas { get; set; }

        public Customer()
        {
            CustomerShoppingAreas = new List<CustomerShoppingArea>();
        }

    }
}

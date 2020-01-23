using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class ShoppingArea
    {
        public int ShoppingAreaId { get; set; }

        public string Descr { get; set; }

        public string Code { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public List<CustomerShoppingArea> CustomerShoppingAreas { get; set; }

        public List<AgentPlusUserShopingArea> AgentPlusUserShopingAreas { get; set; }

        public List<WarehouseShoppingArea> WarehouseShoppingAreas { get; set; }

        public ShoppingArea()
        {
            CustomerShoppingAreas = new List<CustomerShoppingArea>();

            AgentPlusUserShopingAreas = new List<AgentPlusUserShopingArea>();

            WarehouseShoppingAreas = new List<WarehouseShoppingArea>();
        }
    }

    //public class ShoppingAreaEditModel
    //{
    //    public ShoppingArea Area { get; set; }
    //    public ShoppingAreaFilter ShoppingAreaFilter { get; set; }
    //    public IEnumerable<ShoppingAreaItem> ShoppingAreaItems { get; set; }
    //    public PageViewModel PageViewModel { get; set; }
    //    public RouteValueDictionary RouteValue { get; set; }
    //}

    public class ShoppingAreaItem
    {
        public int CustomerId { get; set; }

        public string Descr { get; set; }

        public string Inn { get; set; }

        public string AddressPost { get; set; }

        public string Code { get; set; }

        public bool Selected { get; set; }
    }

    public enum ShoppingAreaFilter
    {        
        All,
        Filter
    }
}
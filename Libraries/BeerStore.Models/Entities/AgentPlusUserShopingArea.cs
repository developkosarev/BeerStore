using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class AgentPlusUserShopingArea
    {        
        public int AgentPlusUserId { get; set; }

        public AgentPlusUser AgentPlusUser { get; set; }
        
        public int ShoppingAreaId { get; set; }

        public ShoppingArea ShoppingArea { get; set; }                
        
    }
    
}

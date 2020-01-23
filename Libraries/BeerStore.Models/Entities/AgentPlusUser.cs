using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class AgentPlusUser
    {
        public int AgentPlusUserId { get; set; }

        public string Code { get; set; }

        public string Descr { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public bool Update { get; set; }

        public DateTime DateOrder { get; set; }

        public DateTime DateUpdate { get; set; }

        public int? MerchandiserId { get; set; }

        public Merchandiser Merchandiser { get; set; }

        public List<AgentPlusUserShopingArea> AgentPlusUserShopingAreas { get; set; }

        public AgentPlusUser()
        {
            AgentPlusUserShopingAreas = new List<AgentPlusUserShopingArea>();
        }
    }
}

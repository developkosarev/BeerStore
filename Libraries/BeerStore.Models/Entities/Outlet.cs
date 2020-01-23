using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class Outlet
    {
        public int OutletId { get; set; }

        public string Code { get; set; }        

        [Required(ErrorMessage = "Please enter a outlet name")]
        public string Descr { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public string CustomerCode { get; set; }

        public string Kpp { get; set; }
        
        public int? CustomerId { get; set; }
        
        public Customer Customer { get; set; }

    }
}

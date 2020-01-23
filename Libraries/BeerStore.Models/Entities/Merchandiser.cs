using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class Merchandiser
    {
        public int MerchandiserId { get; set; }

        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter a merchandiser name")]
        public string Descr { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class Organisation
    {
        public int OrganisationId { get; set; }
        
        public string Descr { get; set; }

        public string Code { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        [Column(TypeName = "varchar(12)")]
        public string Inn { get; set; }        

    }
}

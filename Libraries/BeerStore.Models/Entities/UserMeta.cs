using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{    
    [Table("UserMeta")]
    public class UserMeta
    {        
        [Column("Id")]
        public int Id { get; set; }
        
        [MaxLength(450)]
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }

        [MaxLength(70)]        
        public string FirstName { get; set; }

        [MaxLength(70)]
        public string LastName { get; set; }        

        public int? ShoppingAreaId { get; set; }

        public ShoppingArea ShoppingArea { get; set; }
        
    }
}

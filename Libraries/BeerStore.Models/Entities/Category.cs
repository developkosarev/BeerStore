using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter a category name")]
        public string Descr { get; set; }

        public string Code { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public int ParentCategoryId { get; set; }

        public string ParentCategoryCode { get; set; }
        
        public ICollection<Product> Products { get; set; }

    }
}

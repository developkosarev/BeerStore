using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerStore.Models.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
       
        public string Descr { get; set; }

        public string Code { get; set; }

        public string Version { get; set; }

        public bool IsMark { get; set; }

        public List<Warehouse> Warehouses { get; set; }

        public Department()
        {
            Warehouses = new List<Warehouse>();
        }
    }
}

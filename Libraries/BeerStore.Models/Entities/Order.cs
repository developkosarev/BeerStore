using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeerStore.Models.Entities
{
    public class Order
    {        
        public int OrderId { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(10)]
        public string Number { get; set; }

        [MaxLength(450)]
        public string UserId { get; set; }        
        
        [MaxLength(256)]
        public string FirstName { get; set; }
        
        [MaxLength(256)]
        public string LastName { get; set; }

        public Customer Customer { get; set; }

        public Outlet Outlet { get; set; }

        public Warehouse Warehouse { get; set; }

        public bool Shipped { get; set; }

        public bool ThisReturn { get; set; }

        public bool Finance { get; set; }

        public PayType PayType { get; set; }

        public string Comment { get; set; }

        public ICollection<CartLine> Lines { get; set; }
    }

    public enum PayType
    {
        [Display(Name = "Наличные")]
        Cash,
        [Display(Name = "Безналичные")]
        Cashless,
        [Display(Name = "Отсрочка платежа")]
        DelayOfPayment
    }
}

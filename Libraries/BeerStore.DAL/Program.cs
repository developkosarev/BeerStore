using BeerStore.DAL.EF;
using BeerStore.Models.Entities;
using System;
using System.Collections.Generic;

namespace BeerStore.DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Category();
        }

        private static void Category()
        {
            using (StoreContext storeContext = new StoreContext())
            {
                foreach (Category category in (IEnumerable<Category>)storeContext.Category)
                    Console.WriteLine(string.Format("{0} . {1} - {2}", (object)category.CategoryId, (object)category.Descr, (object)category.Code));
            }
        }
    }
}

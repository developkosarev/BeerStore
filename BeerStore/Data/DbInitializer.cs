using BeerStore.DAL.EF;
using BeerStore.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace BeerStore.Data
{
    public class DbInitializer
    {
        public static RoleManager<IdentityRole> RoleManager { get; set; }
        public static UserManager<ApplicationUser> UserManager { get; set; }

        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();

            AddRoles(context);
            AddUsers(context);

            AddCategories(context);
            AddProducts(context);
            AddOrganisations(context);
            AddDepartments(context);
            AddWarehouses(context);
            AddCustomers(context);
            AddOutlets(context);
        }

        private static void AddRoles(StoreContext context)
        {
            if (RoleManager.RoleExistsAsync("Admins").GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new IdentityRole("Admins")).GetAwaiter().GetResult();
            }

            if (RoleManager.RoleExistsAsync("AdminsApi").GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new IdentityRole("AdminsApi")).GetAwaiter().GetResult();
            }

            if (RoleManager.RoleExistsAsync("Users").GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new IdentityRole("Users")).GetAwaiter().GetResult();
            }            
        }

        private static void AddUsers(StoreContext context)
        {            
            if (UserManager.FindByEmailAsync("admin@example.com").GetAwaiter().GetResult() == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var registerResult = UserManager.CreateAsync(user, "Admin@2018").GetAwaiter().GetResult();

                if (registerResult.Succeeded)
                {
                    UserManager.AddToRoleAsync(user, "Admins").GetAwaiter().GetResult();

                    var shoppingArea = new ShoppingArea
                    {
                        Descr = "Site " + user.LastName + " " + user.FirstName,
                        Code = "",
                        Version = "",
                        IsMark = false
                    };

                    var userMeta = new UserMeta
                    {                        
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ShoppingArea = shoppingArea
                    };

                    context.ShoppingArea.Add(shoppingArea);
                    context.UserMeta.Add(userMeta);
                    context.SaveChanges();

                    user.UserMetaId = userMeta.Id;
                    UserManager.UpdateAsync(user).GetAwaiter().GetResult();
                }
            }
        }


        private static void AddCategories(StoreContext context)
        {
            if (!context.Category.Any())
            {
                context.Category.AddRange(
                    new Category { Code = "S000000001", Descr = "Оборудование", ParentCategoryCode = "", ParentCategoryId = 0 },
                    new Category { Code = "S000000003", Descr = "Балоны", ParentCategoryCode = "S000000001", ParentCategoryId = 0 },
                    new Category { Code = "S000000004", Descr = "Газ", ParentCategoryCode = "S000000001", ParentCategoryId = 0 },
                    new Category { Code = "S000000002", Descr = "Кеги", ParentCategoryCode = "S000000001", ParentCategoryId = 0 }
                );
                context.SaveChanges();

                IEnumerable<Category> category = context.Category.ToList();

                foreach (var item in category)
                {
                    if (item.ParentCategoryCode != "")
                    {
                        Category parent = context.Category.FirstOrDefault(c => c.Code == item.ParentCategoryCode);
                        if (parent != null)
                        {
                            item.ParentCategoryId = parent.CategoryId;

                            context.SaveChanges();
                        }
                    }
                }
            }
        }

        private static void AddProducts(StoreContext context)
        {
            if (!context.Products.Any())
            {
                string CategoryCode = "S000000002";

                var products = new List<Product>()
                {
                    new Product { Code = "S000005897", Descr = "КЕГ 20L CBW EURO SANKEY SENSITIVE (возвратный)" },
                    new Product { Code = "S000005368", Descr = "КЕГ 20л с фитингом A (возвратный)" }
                };

                foreach (var item in products)
                {
                    Category category = context.Category.FirstOrDefault(c => c.Code == CategoryCode);
                    if (category != null)
                    {
                        item.CategoryId = category.CategoryId;
                    }
                }

                context.Products.AddRange(products);
                context.SaveChanges();                
            }
        }

        private static void AddOrganisations(StoreContext context)
        {
            if (!context.Organisations.Any())
            {
                context.Organisations.AddRange(                    
                    new Organisation { Code = "S00000039", Descr = "Аламбик Плюс ООО", IsMark = false, Inn = "6450103776" }
                );
                context.SaveChanges();
            }
        }

        private static void AddDepartments(StoreContext context)
        {
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Code = "S00000007", Descr = "Сокур", IsMark = false }
                );
                context.SaveChanges();
            }
        }
        
        private static void AddWarehouses(StoreContext context)
        {
            if (!context.Warehouses.Any())
            {
                var warehouses = new List<Warehouse>()
                {
                    new Warehouse { Code = "S00000053", Descr = "СКЛАД № 29 (Сокур)", IsMark = false, DepartmentCode = "S00000007" }
                };

                foreach (var item in warehouses)
                {
                    Department department = context.Departments.FirstOrDefault(d => d.Code == item.DepartmentCode);
                    if (department != null)
                    {
                        item.DepartmentId = department.DepartmentId;
                    }
                }

                context.Warehouses.AddRange(warehouses);
                context.SaveChanges();                
            }
        }

        private static void AddCustomers(StoreContext context)
        {
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>()
                {                    
                    new Customer { Code = "546", Descr = "АРТ ЛТД", Inn = "6449012771" },
                    new Customer { Code = "S46000393", Descr = "Бандана Клуб ООО", Inn = "6122018983" }
                };
                
                context.Customers.AddRange(customers);
                context.SaveChanges();
            }
        }

        private static void AddOutlets(StoreContext context)
        {
            if (!context.Outlets.Any())
            {
                var outlets = new List<Outlet>()
                {
                    new Outlet
                    {
                        Code = "S00030138",
                        Descr = "Саратовская обл, Энгельс-19 г, 4 квартал",
                        CustomerCode = "546",
                        Kpp = "644945001"
                    },
                    new Outlet
                    {
                        Code = "S00026256",
                        Descr = "ЭНГЕЛЬС, ТЕЛЬМАНА, 35",
                        CustomerCode = "546",
                        Kpp = "644901001"
                    },

                    new Outlet
                    {
                        Code = "S46000769",
                        Descr = "Ростовская обл, Мясниковский р - н, Крым с, Большесальская ул, Дом 13",
                        CustomerCode = "S46000393",
                        Kpp = "612201001"
                    }
                };

                foreach (var item in outlets)
                {
                    Customer customer = context.Customers.FirstOrDefault(o => o.Code == item.CustomerCode);
                    if (customer != null)
                    {
                        item.CustomerId = customer.CustomerId;
                    }
                }

                context.Outlets.AddRange(outlets);
                context.SaveChanges();
            }
        }

    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using BeerStore.Models.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace BeerStore.DAL.EF
{
    public class StoreContext : IdentityDbContext<ApplicationUser>
    {
        public StoreContext()
        {
            //Database.EnsureCreated();
        }

        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }
        
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Merchandiser> Merchandisers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<ShoppingArea> ShoppingArea { get; set; }

        public DbSet<AgentPlusUser> AgentPlusUsers { get; set; }
        public DbSet<AgentPlusUserShopingArea> AgentPlusUserShopingArea { get; set; }

        public DbSet<CustomerShoppingArea> CustomerShoppingArea { get; set; }
        public DbSet<WarehouseShoppingArea> WarehouseShoppingArea { get; set; }

        public DbSet<MarketingEvent> MarketingEvent { get; set; }
        public DbSet<MarketingEventProduct> MarketingEventProduct { get; set; }

        public DbSet<Order> Orders { get; set; }                        
        
        public DbSet<Stock> Stock { get; set; }        

        public DbSet<UserMeta> UserMeta { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // EnableRetryOnFailure adds default SqlServerRetryingExecutionStrategy
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=ICORE7;Initial Catalog=BeerStore;Persist Security Info=True;User ID=sa;Password=sql1c"
                    , b => b.UseRowNumberForPaging());
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Category>().HasIndex(c => c.Code);
            builder.Entity<Product>().HasIndex(p => p.Code);
            builder.Entity<Customer>().HasIndex(c => c.Code);
            builder.Entity<Outlet>().HasIndex(o => o.Code);
            builder.Entity<Organisation>().HasIndex(o => o.Code);
            builder.Entity<Department>().HasIndex(d => d.Code);

            builder.Entity<Merchandiser>().HasIndex(m => m.Code);
            builder.Entity<Warehouse>().HasIndex(w => w.Code);            
            builder.Entity<ShoppingArea>().HasIndex(s => s.Code);

            builder.Entity<MarketingEvent>().HasIndex(c => c.Code);            
            builder.Entity<Order>().HasIndex(s => s.Number);
            //builder.Entity<TransportModule>().HasIndex(s => s.Code);

            builder.ApplyConfiguration(new CustomerShoppingAreaConfiguration());            

            builder.Entity<AgentPlusUserShopingArea>().HasKey(t => new { t.AgentPlusUserId, t.ShoppingAreaId });
            builder.Entity<AgentPlusUserShopingArea>()
                .HasOne(c => c.AgentPlusUser)
                .WithMany(c => c.AgentPlusUserShopingAreas)
                .HasForeignKey(c => c.AgentPlusUserId);

            builder.Entity<AgentPlusUserShopingArea>()
                .HasOne(s => s.ShoppingArea)
                .WithMany(s => s.AgentPlusUserShopingAreas)
                .HasForeignKey(s => s.ShoppingAreaId);


            builder.Entity<MarketingEventProduct>().HasKey(t => new { t.MarketingEventId, t.ProductId });
            builder.Entity<MarketingEventProduct>()
                .HasOne(c => c.MarketingEvent)
                .WithMany(c => c.MarketingEventProducts)
                .HasForeignKey(c => c.MarketingEventId);

            builder.Entity<MarketingEventProduct>()
                .HasOne(s => s.Product)
                .WithMany(s => s.MarketingEventProducts)
                .HasForeignKey(s => s.ProductId);


            builder.Entity<WarehouseShoppingArea>().HasKey(t => new { t.ShoppingAreaId, t.WarehouseId });
            builder.Entity<WarehouseShoppingArea>()
                .HasOne(c => c.Warehouse)
                .WithMany(c => c.WarehouseShoppingAreas)
                .HasForeignKey(c => c.WarehouseId);

            builder.Entity<WarehouseShoppingArea>()
                .HasOne(s => s.ShoppingArea)
                .WithMany(s => s.WarehouseShoppingAreas)
                .HasForeignKey(s => s.ShoppingAreaId);


            builder.Entity<Stock>().HasKey(t => new { t.ProductId, t.WarehouseId });
            builder.Entity<Stock>()
                .HasOne(c => c.Warehouse)
                .WithMany(c => c.Stocks)
                .HasForeignKey(c => c.WarehouseId);

            builder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(s => s.Stocks)
                .HasForeignKey(s => s.ProductId);

            builder.Entity<ApplicationUser>()
                .HasOne(p => p.UserMeta)
                .WithOne(i => i.User)
                .HasForeignKey<UserMeta>(b => b.UserId);


        }
    }
}

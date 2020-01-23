﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BeerStore.DAL.EF.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20191007095839_UpdateProducts")]
    partial class UpdateProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BeerStore.Data.Entities.AgentPlusUser", b =>
                {
                    b.Property<int>("AgentPlusUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<DateTime>("DateOrder");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<string>("Descr");

                    b.Property<bool>("IsMark");

                    b.Property<int?>("MerchandiserId");

                    b.Property<bool>("Update");

                    b.Property<string>("Version");

                    b.HasKey("AgentPlusUserId");

                    b.HasIndex("MerchandiserId");

                    b.ToTable("AgentPlusUsers");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.AgentPlusUserShopingArea", b =>
                {
                    b.Property<int>("AgentPlusUserId");

                    b.Property<int>("ShoppingAreaId");

                    b.HasKey("AgentPlusUserId", "ShoppingAreaId");

                    b.HasIndex("ShoppingAreaId");

                    b.ToTable("AgentPlusUserShopingArea");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasMaxLength(70);

                    b.Property<string>("LastName")
                        .HasMaxLength(70);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("RefreshTokenHash");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<int?>("UserMetaId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.CartLine", b =>
                {
                    b.Property<int>("CartLineId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MarketingEventId");

                    b.Property<int?>("OrderId");

                    b.Property<int?>("ProductId");

                    b.Property<int>("Quantity");

                    b.HasKey("CartLineId");

                    b.HasIndex("MarketingEventId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrdersLine");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Descr")
                        .IsRequired();

                    b.Property<bool>("IsMark");

                    b.Property<string>("ParentCategoryCode");

                    b.Property<int>("ParentCategoryId");

                    b.Property<string>("Version");

                    b.HasKey("CategoryId");

                    b.HasIndex("Code");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressPost");

                    b.Property<string>("Code");

                    b.Property<string>("Descr");

                    b.Property<string>("Inn");

                    b.Property<bool>("IsMark");

                    b.Property<string>("Version");

                    b.HasKey("CustomerId");

                    b.HasIndex("Code");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.CustomerShoppingArea", b =>
                {
                    b.Property<int>("ShoppingAreaId");

                    b.Property<int>("CustomerId");

                    b.Property<int>("UpdateStatus");

                    b.HasKey("ShoppingAreaId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerShoppingArea");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Descr");

                    b.Property<bool>("IsMark");

                    b.Property<string>("Version");

                    b.HasKey("DepartmentId");

                    b.HasIndex("Code");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.MarketingEvent", b =>
                {
                    b.Property<int>("MarketingEventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Descr");

                    b.Property<bool>("IsMark");

                    b.Property<string>("Version");

                    b.HasKey("MarketingEventId");

                    b.HasIndex("Code");

                    b.ToTable("MarketingEvent");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.MarketingEventProduct", b =>
                {
                    b.Property<int>("MarketingEventId");

                    b.Property<int>("ProductId");

                    b.HasKey("MarketingEventId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("MarketingEventProduct");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Merchandiser", b =>
                {
                    b.Property<int>("MerchandiserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Descr")
                        .IsRequired();

                    b.Property<bool>("IsMark");

                    b.Property<string>("Version");

                    b.HasKey("MerchandiserId");

                    b.HasIndex("Code");

                    b.ToTable("Merchandisers");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int?>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Finance");

                    b.Property<string>("FirstName")
                        .HasMaxLength(256);

                    b.Property<string>("LastName")
                        .HasMaxLength(256);

                    b.Property<string>("Number")
                        .HasMaxLength(10);

                    b.Property<int?>("OutletId");

                    b.Property<int>("PayType");

                    b.Property<bool>("Shipped");

                    b.Property<bool>("ThisReturn");

                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.Property<int?>("WarehouseId");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Number");

                    b.HasIndex("OutletId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Organisation", b =>
                {
                    b.Property<int>("OrganisationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Descr");

                    b.Property<string>("Inn")
                        .HasColumnType("varchar(12)");

                    b.Property<bool>("IsMark");

                    b.Property<string>("Version");

                    b.HasKey("OrganisationId");

                    b.HasIndex("Code");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Outlet", b =>
                {
                    b.Property<int>("OutletId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("CustomerCode");

                    b.Property<int?>("CustomerId");

                    b.Property<string>("Descr")
                        .IsRequired();

                    b.Property<bool>("IsMark");

                    b.Property<string>("Kpp");

                    b.Property<string>("Version");

                    b.HasKey("OutletId");

                    b.HasIndex("Code");

                    b.HasIndex("CustomerId");

                    b.ToTable("Outlets");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryId");

                    b.Property<string>("Code");

                    b.Property<string>("Descr")
                        .IsRequired();

                    b.Property<bool>("IsMark");

                    b.Property<int>("Pack");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(15,2)");

                    b.Property<decimal>("Price1")
                        .HasColumnType("numeric(15,2)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric(20,3)");

                    b.Property<decimal>("Quantity1")
                        .HasColumnType("numeric(20,3)");

                    b.Property<string>("Version");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Code");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.ShoppingArea", b =>
                {
                    b.Property<int>("ShoppingAreaId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Descr");

                    b.Property<bool>("IsMark");

                    b.Property<string>("Version");

                    b.HasKey("ShoppingAreaId");

                    b.HasIndex("Code");

                    b.ToTable("ShoppingArea");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Stock", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("WarehouseId");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric(20,3)");

                    b.Property<decimal>("ReservedQuantity")
                        .HasColumnType("numeric(20,3)");

                    b.HasKey("ProductId", "WarehouseId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.UserMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("FirstName")
                        .HasMaxLength(70);

                    b.Property<string>("LastName")
                        .HasMaxLength(70);

                    b.Property<int?>("ShoppingAreaId");

                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("ShoppingAreaId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("UserMeta");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("WarehouseId");

                    b.Property<string>("Code");

                    b.Property<string>("DepartmentCode");

                    b.Property<int?>("DepartmentId");

                    b.Property<string>("Descr")
                        .IsRequired();

                    b.Property<bool>("IsMark");

                    b.Property<string>("Version");

                    b.HasKey("WarehouseId");

                    b.HasIndex("Code");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Warehouse");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.WarehouseShoppingArea", b =>
                {
                    b.Property<int>("ShoppingAreaId");

                    b.Property<int>("WarehouseId");

                    b.Property<int>("UpdateStatus");

                    b.HasKey("ShoppingAreaId", "WarehouseId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("WarehouseShoppingArea");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.AgentPlusUser", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.Merchandiser", "Merchandiser")
                        .WithMany()
                        .HasForeignKey("MerchandiserId");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.AgentPlusUserShopingArea", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.AgentPlusUser", "AgentPlusUser")
                        .WithMany("AgentPlusUserShopingAreas")
                        .HasForeignKey("AgentPlusUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BeerStore.Data.Entities.ShoppingArea", "ShoppingArea")
                        .WithMany("AgentPlusUserShopingAreas")
                        .HasForeignKey("ShoppingAreaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BeerStore.Data.Entities.CartLine", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.MarketingEvent", "MarketingEvent")
                        .WithMany()
                        .HasForeignKey("MarketingEventId");

                    b.HasOne("BeerStore.Data.Entities.Order")
                        .WithMany("Lines")
                        .HasForeignKey("OrderId");

                    b.HasOne("BeerStore.Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.CustomerShoppingArea", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.Customer", "Customer")
                        .WithMany("CustomerShoppingAreas")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BeerStore.Data.Entities.ShoppingArea", "ShoppingArea")
                        .WithMany("CustomerShoppingAreas")
                        .HasForeignKey("ShoppingAreaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BeerStore.Data.Entities.MarketingEventProduct", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.MarketingEvent", "MarketingEvent")
                        .WithMany("MarketingEventProducts")
                        .HasForeignKey("MarketingEventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BeerStore.Data.Entities.Product", "Product")
                        .WithMany("MarketingEventProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Order", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("BeerStore.Data.Entities.Outlet", "Outlet")
                        .WithMany()
                        .HasForeignKey("OutletId");

                    b.HasOne("BeerStore.Data.Entities.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Outlet", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.Customer", "Customer")
                        .WithMany("Outlets")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Product", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Stock", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.Product", "Product")
                        .WithMany("Stocks")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BeerStore.Data.Entities.Warehouse", "Warehouse")
                        .WithMany("Stocks")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BeerStore.Data.Entities.UserMeta", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.ShoppingArea", "ShoppingArea")
                        .WithMany()
                        .HasForeignKey("ShoppingAreaId");

                    b.HasOne("BeerStore.Data.Entities.ApplicationUser", "User")
                        .WithOne("UserMeta")
                        .HasForeignKey("BeerStore.Data.Entities.UserMeta", "UserId");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.Warehouse", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.Department", "Department")
                        .WithMany("Warehouses")
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("BeerStore.Data.Entities.WarehouseShoppingArea", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.ShoppingArea", "ShoppingArea")
                        .WithMany("WarehouseShoppingAreas")
                        .HasForeignKey("ShoppingAreaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BeerStore.Data.Entities.Warehouse", "Warehouse")
                        .WithMany("WarehouseShoppingAreas")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BeerStore.Data.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BeerStore.Data.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
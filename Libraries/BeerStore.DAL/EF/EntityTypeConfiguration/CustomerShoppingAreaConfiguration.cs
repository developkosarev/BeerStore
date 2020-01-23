using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using BeerStore.Models.Entities;

namespace BeerStore.DAL.EF
{
    public class CustomerShoppingAreaConfiguration : IEntityTypeConfiguration<CustomerShoppingArea>
    {
        public void Configure(EntityTypeBuilder<CustomerShoppingArea> builder)
        {
            builder.HasKey(t => new { t.ShoppingAreaId, t.CustomerId });
            builder.HasOne(c => c.Customer)
                .WithMany(c => c.CustomerShoppingAreas)
                .HasForeignKey(c => c.CustomerId);

            builder.HasOne(s => s.ShoppingArea)
                .WithMany(s => s.CustomerShoppingAreas)
                .HasForeignKey(s => s.ShoppingAreaId);

        }
    }
}

using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class BikeConfiguration : IEntityTypeConfiguration<Bike>
    {
        public void Configure(EntityTypeBuilder<Bike> builder)
        {
            builder.HasIndex(b => b.SerialNumber).IsUnique();
            builder.Property(b => b.SerialNumber).HasMaxLength(20).IsRequired();
            builder.Property(b => b.Make).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Model).HasMaxLength(100).IsRequired();
            builder.Property(b => b.PurchasedFrom).HasMaxLength(250).IsRequired();
        }
    }
}
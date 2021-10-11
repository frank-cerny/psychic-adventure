using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class NonCapitalItemConfiguration : IEntityTypeConfiguration<NonCapitalItem>
    {
        public void Configure(EntityTypeBuilder<NonCapitalItem> builder)
        {
            builder.Property(nci => nci.UnitCost).IsRequired();
            builder.Property(nci => nci.UnitsPurchased).IsRequired();
            builder.Property(nci => nci.UnitsRemaining).IsRequired();
            builder.Property(nci => nci.Name).HasMaxLength(100).IsRequired();
            builder.Property(nci => nci.Description).HasMaxLength(250);
            builder.HasMany(nci => nci.ExpenseItems)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
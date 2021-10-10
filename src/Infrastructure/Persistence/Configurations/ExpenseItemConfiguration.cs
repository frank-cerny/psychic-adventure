using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class ExpenseItemConfiguration : IEntityTypeConfiguration<ExpenseItem>
    {
        public void Configure(EntityTypeBuilder<ExpenseItem> builder)
        {
            builder.Property(e => e.UnitCost).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(250);
            builder.HasIndex(e => new {e.Name, e.DatePurchased}).IsUnique();
        }
    }
}
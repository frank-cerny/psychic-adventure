using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class ExpenseItemConfiguration : IEntityTypeConfiguration<ExpenseItem>
    {
        public void Configure(EntityTypeBuilder<ExpenseItem> builder)
        {
            builder.Property(nci => nci.UnitCost).IsRequired();
            builder.Property(nci => nci.Name).HasMaxLength(100).IsRequired();
            builder.Property(nci => nci.Description).HasMaxLength(250);
        }
    }
}
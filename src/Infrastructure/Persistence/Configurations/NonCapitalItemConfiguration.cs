using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class NonCapitalItemConfiguration : IEntityTypeConfiguration<NonCapitalItem>
    {
        public void Configure(EntityTypeBuilder<NonCapitalItem> builder)
        {
            builder.Property(nci => nci.Cost).IsRequired();
            builder.Property(nci => nci.Name).HasMaxLength(100).IsRequired();
            builder.Property(nci => nci.Description).HasMaxLength(250).IsRequired();
        }
    }
}
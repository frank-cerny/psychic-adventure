using Microsoft.EntityFrameworkCore;
using bike_selling_app.Application.Common.Interfaces;

namespace bike_selling_app.Infrastructure.Persistence
{
    public class ApplicationDbContextMySql : ApplicationDbContext
    {
        public ApplicationDbContextMySql(
            DbContextOptions<ApplicationDbContextMySql> options, 
            IDateTime dateTime,
            IDomainEventService domainEventService) : base (options, domainEventService, dateTime)
        {

        }
    }
}
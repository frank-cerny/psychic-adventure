using Microsoft.EntityFrameworkCore;
using bike_selling_app.Application.Common.Interfaces;

namespace bike_selling_app.Infrastructure.Persistence
{
    public class ApplicationDbContextSqlServer : ApplicationDbContext
    {
        public ApplicationDbContextSqlServer(
            DbContextOptions<ApplicationDbContextSqlServer> options, 
            IDateTime dateTime,
            IDomainEventService domainEventService) : base (options, domainEventService, dateTime)
        {

        }
    }
}
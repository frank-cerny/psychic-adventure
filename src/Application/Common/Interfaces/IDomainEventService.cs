using bike_selling_app.Domain.Common;
using System.Threading.Tasks;

namespace bike_selling_app.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}

using bike_selling_app.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace bike_selling_app.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> AddBike(Bike bike);
        Task RemoveBike(int bikeId);
        Task<IList<Bike>> GetAllBikes();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace bike_selling_app.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> AddBike(Bike bike);
        Task RemoveBike(Bike bike);
        Task<Bike> GetBikeById(int bikeId);
        Task DeleteBikeById(int bikeId);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

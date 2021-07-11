using bike_selling_app.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace bike_selling_app.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}

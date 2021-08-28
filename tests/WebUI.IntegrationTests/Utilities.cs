using bike_selling_app.Infrastructure.Persistence;
using bike_selling_app.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace bike_selling_app.WebUI.IntegrationTests
{
    public static class Utilities
    {
        public static void SeedDbForTests(ApplicationDbContext context)
        {
            // Add bikes
            Bike bike1 = new Bike
            {
                SerialNumber = "2124893",
                Make = "Schwinn",
                Model = "Tempo",
                PurchasePrice = 21.45,
                PurchasedFrom = "Facebook Marketplace",
                DatePurchased = System.DateTime.Today
            };
            Bike bike2 = new Bike
            {
                SerialNumber = "1236721-882, 89293, 899921",
                Make = "Nishiki",
                Model = "International",
                PurchasePrice = 34.56,
                PurchasedFrom = "Ebay",
                DatePurchased = System.DateTime.Parse("2020-08-02")
            };
            context.Bikes.Add(bike1);
            context.Bikes.Add(bike2);
            context.SaveChanges();
        }
    }
}
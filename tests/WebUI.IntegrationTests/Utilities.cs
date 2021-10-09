using bike_selling_app.Infrastructure.Persistence;
using bike_selling_app.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            // Add test project
            Project project = new Project
            {
                Title = "Test Project",
                Description = "A simple test project!",
                DateStarted = System.DateTime.Parse("2020-09-15"),
                Bikes = new List<Bike>() { bike1 }
            };
            context.Bikes.Add(bike1);
            context.Bikes.Add(bike2);
            context.Projects.Add(project);
            context.SaveChanges();
        }

        public static void ClearDb(ApplicationDbContext context)
        {
            context.Bikes.Clear();
            context.ExpenseItems.Clear();
            context.RevenueItems.Clear();
            context.CapitalItems.Clear();
            context.NonCapitalItems.Clear();
            context.Projects.Clear();
            context.SaveChanges();
        }
    }
}
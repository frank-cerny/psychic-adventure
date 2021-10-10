using bike_selling_app.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace bike_selling_app.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Add a test bike
            if (context.Bikes.SingleOrDefault(b => b.SerialNumber.Equals("2124893")) == null)
            {
                Bike b = new Bike 
                {
                    SerialNumber = "2124893",
                    Make = "Schwinn",
                    Model = "Tempo",
                    PurchasePrice = 21.45,
                    PurchasedFrom = "Facebook Marketplace",
                    DatePurchased = System.DateTime.Today
                };
                context.Bikes.Add(b);
                // This is required after every entity is added, we are not guranteed the same context instance throughout this file? Was having
                // issues where the bike could not be found in the project below
                await context.SaveChangesAsync();
            }
            // Add a test project
            if (context.Projects.SingleOrDefault(p => p.Title.Equals("Test Project")) == null)
            {
                var bike = context.Bikes.SingleOrDefault(b => b.SerialNumber.Equals("2124893"));
                Project p = new Project
                {
                    Title = "Test Project",
                    Description = "A simple test project!",
                    DateStarted = System.DateTime.Parse("2020-09-15"),
                    Bikes = new List<Bike>() { bike }
                };
                context.Projects.Add(p);
                await context.SaveChangesAsync();
            }
            // Add a test capital item
            if (context.CapitalItems.SingleOrDefault(c => c.Name.Equals("TestItem")) == null)
            {
                var item = new CapitalItem
                {
                    Name = "TestItem",
                    Cost = 4.75,
                    Description = "MyDescription!"
                };
                context.CapitalItems.Add(item);
                await context.SaveChangesAsync();
            }
        }
    }
}

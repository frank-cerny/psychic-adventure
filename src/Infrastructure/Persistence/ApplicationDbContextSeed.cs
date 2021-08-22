﻿using bike_selling_app.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace bike_selling_app.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Ensure seed data does not already exist
            if (context.Bikes.SingleOrDefault(b => b.Make.Equals("Schwinn")) == null)
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
            }
            await context.SaveChangesAsync();
        }
    }
}

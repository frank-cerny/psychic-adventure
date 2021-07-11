using System.Collections.Generic;
using System;

namespace bike_selling_app.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public Bike Bike { get; set; }
        public int BikeId { get; set; }
        public IList<CapitalItem> CapitalItems { get; set; }
        public IList<NonCapitalItem> NonCapitalItems { get; set; }
        public IList<RevenueItem> RevenueItems { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateEnded { get; set; }
        public double NetValue { get; set; }
    }
}
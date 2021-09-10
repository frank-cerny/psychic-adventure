using System.Collections.Generic;
using System;

namespace bike_selling_app.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IList<Bike> Bikes { get; set; } = new List<Bike>();
        public IList<CapitalItem> CapitalItems { get; set; } = new List<CapitalItem>();
        public IList<NonCapitalItem> NonCapitalItems { get; set; } = new List<NonCapitalItem>();
        public IList<RevenueItem> RevenueItems { get; set; } = new List<RevenueItem>();
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        // TODO - Do not store this in database, make this a computed entity
        public double NetValue { get; set; } = 0.0;
    }
}
using System.Collections.Generic;

namespace bike_selling_app.Domain.Entities
{
    public class Project
    {
        public Bike bike { get; set; }
        public IList<CapitalItem> capitalItems { get; set; }
        public IList<NonCapitalItem> nonCapitalItems { get; set; }
        public IList<RevenueItem> revenueItems { get; set; }
        // TODO Add date started, and ended
        public double netValue { get; set; }
    }
}
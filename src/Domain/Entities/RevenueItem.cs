using System;
using System.Collections.Generic;

namespace bike_selling_app.Domain.Entities
{
    public class RevenueItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatePurchased { get; set; }
        public double SalePrice { get; set; }
        public string PlatformSoldOn { get; set; }
        public string RevenueItemType { get; set; }
        public bool IsPending { get; set; }
        public double Weight { get; set; }
        public DateTime DateSold { get; set; }
    }
}
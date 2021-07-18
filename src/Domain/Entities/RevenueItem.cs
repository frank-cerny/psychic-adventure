using System;

namespace bike_selling_app.Domain.Entities
{
    public class RevenueItem : Item
    {
        public double SalePrice { get; set; }
        public string PlatformSoldOn { get; set; }
        public string ItemType { get; set; }
        public bool IsPending { get; set; }
        public double Weight { get; set; }
        public DateTime DateSold { get; set; }
    }
}
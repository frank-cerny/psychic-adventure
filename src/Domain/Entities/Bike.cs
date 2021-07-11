using System;

namespace bike_selling_app.Domain.Entities
{
    public class Bike
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double PurchasePrice { get; set; }
        public string PurchasedFrom { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public DateTime DatePurchased { get; set; }
    }
}
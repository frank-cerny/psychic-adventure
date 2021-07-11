using System;

namespace bike_selling_app.Domain.Entities
{
    public abstract class Item
    {
        public int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        public DateTime DatePurchased { get; set; }
    }
}
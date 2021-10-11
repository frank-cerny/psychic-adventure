using System;

namespace bike_selling_app.Domain.Entities
{
    public class ExpenseItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatePurchased { get; set; }
        public double UnitCost { get; set; }
        public int Units { get; set; }
        public int? CapitalItemId { get; set; }
        public int? NonCapitalItemId { get; set; }
        public int? RevenueItemId { get; set; }
    }
}
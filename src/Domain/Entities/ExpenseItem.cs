namespace bike_selling_app.Domain.Entities
{
    public class ExpenseItem : Item
    {
        public double UnitCost { get; set; }
        public int Units { get; set; }
        public int? CapitalItemId { get; set; }
        public int? NonCapitalItemId { get; set; }
        public int? RevenueItemId { get; set; }
    }
}
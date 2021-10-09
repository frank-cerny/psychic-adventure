namespace bike_selling_app.Domain.Entities
{
    public class ExpenseItem : Item
    {
        public double UnitCost { get; set; }
        public int Units { get; set; }
        public int ParentItemId { get; set; }
        public Item ParentItem { get; set; }
    }
}
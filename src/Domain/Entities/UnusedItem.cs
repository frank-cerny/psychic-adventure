using System.Collections.Generic;

namespace bike_selling_app.Domain.Entities
{
    public class UnusedItem : Item
    {
        public double UnitCost { get; set; }
        public int Units { get; set; }
        public string Reason { get; set; }
        public IList<ExpenseItem> ExpenseItems { get; set; }
    }
}
using System.Collections.Generic;

namespace bike_selling_app.Domain.Entities
{
    public class NonCapitalItem : Item
    {
        public Project Project { get; set; }
        public double UnitCost { get; set; }
        public int Units { get; set; }
        public IList<ExpenseItem> ExpenseItems { get; set; }
    }
}
using System.Collections.Generic;

namespace bike_selling_app.Domain.Entities
{
    public class CapitalItem : Item
    {
        public IList<Project> Projects { get; set; }
        public double Cost { get; set; }
        public int UsageCount { get; set; }
        public IList<ExpenseItem> ExpenseItems { get; set; }
    }
}
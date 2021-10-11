using System.Collections.Generic;
using System;

namespace bike_selling_app.Domain.Entities
{
    public class NonCapitalItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatePurchased { get; set; }
        public Project Project { get; set; }
        public int? ProjectId { get; set; }
        public double UnitCost { get; set; }
        public int Units { get; set; }
        public IList<ExpenseItem> ExpenseItems { get; set; }
    }
}
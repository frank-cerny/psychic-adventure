using System.Collections.Generic;
using System;

namespace bike_selling_app.Domain.Entities
{
    public class CapitalItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatePurchased { get; set; }
        public IList<Project> Projects { get; set; }
        public double Cost { get; set; }
        public IList<ExpenseItem> ExpenseItems { get; set; }

        // Computed Properties (Not stored in the database)
        public int UsageCount { get {
            if (this.Projects != null)
            {
                return this.Projects.Count;
            }
            return 0;
        } }
    }
}
using System.Collections.Generic;

namespace bike_selling_app.Domain.Entities
{
    public class CapitalItem : Item
    {
        public IList<Project> projects { get; set; }
        public double cost { get; set; }
    }
}
namespace bike_selling_app.Domain.Entities
{
    public class NonCapitalItem : Item
    {
        public Project project { get; set; }
        public double cost { get; set; }
    }
}
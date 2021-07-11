namespace bike_selling_app.Domain.Entities
{
    public class NonCapitalItem : Item
    {
        public Project Project { get; set; }
        public double Cost { get; set; }
    }
}
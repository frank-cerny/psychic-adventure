namespace bike_selling_app.Domain.Entities
{
    public class UnusedItem : Item
    {
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
    }
}
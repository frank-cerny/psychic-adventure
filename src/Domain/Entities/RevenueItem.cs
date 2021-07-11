namespace bike_selling_app.Domain.Entities
{
    public class RevenueItem : Item
    {
        public string description { get; set; }
        public double salePrice { get; set; }
        public string platformSoldOn { get; set; }
        public string itemType { get; set; }
        public bool isPending { get; set; }
        public double weight { get; set; }
    }
}
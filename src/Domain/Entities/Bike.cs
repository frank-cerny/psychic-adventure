namespace bike_selling_app.Domain.Entities
{
    public class Bike
    {
        public string serialNumber { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public float purchasePrice { get; set; }
        public string purchasedFrom { get; set; }
        public Project project { get; set; }
        public int projectId { get; set; }
        // TODO Add Datetime purchased
    }
}
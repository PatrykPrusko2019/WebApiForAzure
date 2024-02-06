namespace WebApplication1.Models
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string EAN { get; set; }
        public string ProducerName { get; set; }
        public string Category { get; set; }
        public string DefaultImage { get; set; }
        public string Available { get; set; }
        public string Shipping { get; set; }
        public string IsWire { get; set;}
    }
}

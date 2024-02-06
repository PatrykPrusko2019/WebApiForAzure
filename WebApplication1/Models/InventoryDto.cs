namespace WebApplication1.Models
{
    public class InventoryDto
    {
        public string? Shipping { get; set; } // looking for 24h
        public double? ShippingCost { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; }
    }
}

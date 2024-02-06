namespace WebApplication1.Models
{
    public class ShowProductDto
    {
        public string Name { get; set; } // a
        public string EAN { get; set; } // b
        public string ProducerName { get; set; } // c
        public string Category { get; set; } // d
        public string DefaultImage { get; set; } // e
        public string Available { get; set; } // f
        public string SKU { get; set; } // g

        public double ShippingCost { get; set; } // i InventoryDto

        public double NettProductPrice { get; set; } // h-> first part, price netto 
        public double NettProductPriceAfterDiscountForProductLogisticUnit { get; set; } // h-> second part, price netto after discount
    }
}

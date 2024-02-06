using WebApplication1.Models;
using System.IO;

namespace WebApplication1.DownloadDataFromCSV
{
    public static class DownloadData
    {
        public static List<ProductDto> GetDataFromProductsCSV(string product)
        {
            string path = @$"FilesCSV\{product}.csv";
            if (!System.IO.File.Exists(path)) return new List<ProductDto>();

            var values = System.IO.File.ReadAllLines(path)
                                       .Skip(1)
                                       .Select(p => FromProductCSV(p))
                                       .ToList();
             values = values.Where(p => p.Shipping == "24h" && p.IsWire == "0").ToList();

            return values;

        }

        public static ProductDto FromProductCSV(string csvLine)
        {
            string[] values = csvLine.Split("\",\"");
            if (values.Length < 19) values = csvLine.Split("\";\"");
            if (values.Length < 19) return new ProductDto();

            values[0] = values[0].Replace("\"", "");
            values[18] = values[18].Replace("\"", "");

            ProductDto productValues = new ProductDto();

            productValues.ProductId = Convert.ToString(values[0]);
            productValues.SKU = Convert.ToString(values[1]);
            productValues.Name = Convert.ToString(values[2]);
            productValues.EAN = Convert.ToString(values[4]);
            productValues.ProducerName = Convert.ToString(values[6]);
            productValues.Category = Convert.ToString(values[7]);
            productValues.IsWire = Convert.ToString(values[8]);
            productValues.Shipping = Convert.ToString(values[9]);
            productValues.Available = Convert.ToString(values[11]);
            productValues.DefaultImage = Convert.ToString(values[18]);
            return productValues;
        }

        public static List<InventoryDto> GetDataFromInventoriesCSV(string name)
        {
            string path = @$"FilesCSV\{name}.csv";
            if (!System.IO.File.Exists(path)) return new List<InventoryDto>();

                var values = System.IO.File.ReadAllLines(path)
                                           .Skip(1)
                                           .Select(i => FromInventoryCSV(i))
                                           .ToList();
                values = values.Where(i => i.Shipping == "24h").ToList();

                return values;
            
        }

        public static InventoryDto FromInventoryCSV(string csvLine)
        {
            string[] values = csvLine.Split(',');
            if (values.Length < 8) values = csvLine.Split(';');
            values[6] = values[6].Replace("\"", "");
            
            InventoryDto inventoryValues = new InventoryDto();
            
            inventoryValues.ProductId = Convert.ToInt32(values[0]);
            inventoryValues.SKU = Convert.ToString(values[1]);
            inventoryValues.Shipping = Convert.ToString(values[6]);
            inventoryValues.ShippingCost = Double.TryParse(values[7].ToString(), out double result) == true ? result : default;
            return inventoryValues;
        }

        

        public static List<PriceDto> GetDataFromPricesCSV(string price)
        {
            string path = @$"FilesCSV\{price}.csv";
            if (!System.IO.File.Exists(path)) return new List<PriceDto>();

            var values = System.IO.File.ReadAllLines(path)
                                       .Select(p => FromPriceCSV(p))
                                       .ToList();

            return values;
        }


        public static PriceDto FromPriceCSV(string csvLine)
        {
            string[] values = csvLine.Split("\",\"");
            if (values.Length < 6) values = csvLine.Split("\";\"");
            if (values.Length < 6) return new PriceDto();

            values[0] = values[0].Replace("\"", "");
            values[5] = values[5].Replace("\" 0", "");
            values[5] = values[5].Replace("\"", "");

            PriceDto priceValues = new PriceDto();

            priceValues.SKU = Convert.ToString(values[1]);
            string nettProductPrice = values[2].ToString().Replace(",", ".");
            priceValues.NettProductPrice = Double.TryParse(nettProductPrice, out double result) == true ? result : default;
            string nettProductPriceAfterDiscount = values[5].ToString().Replace(",", ".");
            priceValues.NettProductPriceAfterDiscountForProductLogisticUnit = Double.TryParse(nettProductPriceAfterDiscount, out double result2) == true ? result2 : default;
            return priceValues;
        }

    }
}

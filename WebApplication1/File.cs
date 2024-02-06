using WebApplication1.Models;

namespace WebApplication1
{
    public class File
    {
        
        public string Name { get; set; }
        public string UrlAddress { get; set; }
        public List<File> Files { get; set; }
        public static List<ProductDto> Products { get; set; }
        public static List<InventoryDto> Inventories { get; set; }
        public static List<PriceDto> Prices { get; set; }


        public void SetFiles()
        {
             Files = new List<File>()
            {
                new File()
                {
                    Name = "Products",
                    UrlAddress = @"https://rekturacjazadanie.blob.core.windows.net/zadanie/Products.csv",
                },
                new File()
                {
                    Name = "Inventory",
                    UrlAddress = @"https://rekturacjazadanie.blob.core.windows.net/zadanie/Inventory.csv",
                },
                new File()
                {
                    Name = "Prices",
                    UrlAddress = @"https://rekturacjazadanie.blob.core.windows.net/zadanie/Prices.csv",
                }
            };
        }
    }
}

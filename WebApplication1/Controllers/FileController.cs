using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicStoreApi.Exceptions;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/file")]
    public class FileController : ControllerBase
    {
        private IProductService productService;
        private IInventoryService inventoryService;
        private IPriceService priceService;
        private IMapper mapper;


        public FileController(IProductService productService, IInventoryService inventoryService, IPriceService priceService, IMapper mapper)
        {
            this.productService = productService;
            this.inventoryService = inventoryService;
            this.priceService = priceService;
            this.mapper = mapper;
        }


        // first endpoint
        [HttpGet]
        public ActionResult GetFileAndSetDataInDatabase()
        {

            //Downloads 3 files: Products, Inventory, Prices 
            File files = new File();
            files.SetFiles();
            var products = files.Files[0];
            productService.GetCVS(products);

            var inventories = files.Files[1];
            inventoryService.GetCVS(inventories);

            var prices = files.Files[2];
            priceService.GetCVS(prices);


            //Sets data to three lists: Products, Inventories, Prices

            WebApplication1.File.Products = productService.GetData(products.Name);

            WebApplication1.File.Inventories = inventoryService.GetData(inventories.Name);

            WebApplication1.File.Prices = priceService.GetData(prices.Name);

            //Sets data to three new tables: Products, Inventories, Prices

             productService.SetDataInDatabase(WebApplication1.File.Products);

             inventoryService.SetDataInDatabase(WebApplication1.File.Inventories);
            
             priceService.SetDataInDatabase(WebApplication1.File.Prices);

            return Ok("3 tables were created: Products, Inventories, Prices and filled with records !!!");
        }

        // second endpoint
        [HttpGet("details")]
        public ActionResult<string> GetDetailsProduct([FromQuery] string SKU)
        {
            if (WebApplication1.File.Products == null || WebApplication1.File.Products.Count() == 0) return Ok("No records in the Product list!!! use the api/file endpoint -> to load the Products, Inventories, Prices lists and create 3 new tables with records and then api/file/details -> to see the details of a given product");

            var productDetails = productService.GetDetailsProduct(SKU);
            try 
            {
                if (productDetails.SKU == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex) { return Ok("There is no such Product with the given SKU number!!, please enter a valid SKU value"); }
            

            productDetails = inventoryService.GetDetailsInventory(productDetails);

            productDetails = priceService.GetDetailsPrice(productDetails);

            var showRecord = mapper.Map<ShowProductDto>(productDetails);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(showRecord.Name);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.EAN);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.ProducerName);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.Category);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.DefaultImage);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.Available);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.SKU);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.ShippingCost);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.NettProductPrice);
            stringBuilder.Append(";");
            stringBuilder.Append(showRecord.NettProductPriceAfterDiscountForProductLogisticUnit);

            return (stringBuilder.ToString());
        }
    }
}

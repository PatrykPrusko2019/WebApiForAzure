using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ShowAll;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AllProductDto>> GetAll([FromQuery] ProductQuery productQuery)
        {
            var productsDtos = productService.GetAll(productQuery);

            return Ok(productsDtos);
        }
    }
}

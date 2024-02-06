using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.ShowAll;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/price")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private IPriceService priceService;

        public PriceController(IPriceService priceService)
        {
            this.priceService = priceService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AllPriceDto>> GetAll([FromQuery] PriceQuery priceQuery)
        {
            var pricesDtos = priceService.GetAll(priceQuery);

            return Ok(pricesDtos);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.ShowAll;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private IInventoryService inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AllInventoryDto>> GetAll([FromQuery] InventoryQuery inventoryQuery)
        {
            var inventoriesDtos = inventoryService.GetAll(inventoryQuery);

            return Ok(inventoriesDtos);
        }
    }
}

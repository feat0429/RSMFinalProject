using Microsoft.AspNetCore.Mvc;
using RSMFinalProject.BLL.Services.Contract;

namespace RSMFinalProject.API.Controllers
{
    [Route("api/sales-territory")]
    [ApiController]
    public class SalesTerritoryController : ControllerBase
    {
        private readonly ISalesTerritoryService _salesTerritoryService;

        public SalesTerritoryController(ISalesTerritoryService salesTerritoryService)
        {
            _salesTerritoryService = salesTerritoryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllSalesTerritories()
        {
            var _salesTerritoryDtos = await _salesTerritoryService.GetAllSalesTerritories();

            return Ok(_salesTerritoryDtos);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RSMFinalProject.BLL.Services.Contract;
using RSMFinalProject.DTO.Territory;

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

        /// <summary>
        /// Gets all sales territories in the database.
        /// </summary>
        /// <returns>The name and id of each Sales Territory.</returns>
        /// <response code="200">Returns found territories. It can return an empty array.</response>        
        [HttpGet]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(IEnumerable<GetAllSalesTerritoriesDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [Route("")]
        public async Task<IActionResult> GetAllSalesTerritories()
        {
            var _salesTerritoryDtos = await _salesTerritoryService.GetAllSalesTerritories();

            return Ok(_salesTerritoryDtos);
        }
    }
}

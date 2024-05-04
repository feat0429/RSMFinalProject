namespace RSMFinalProject.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RSMFinalProject.BLL.Services.Contract;
    using RSMFinalProject.DTO.Pagination;
    using RSMFinalProject.DTO.SalesOrderHeader;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;

    [Route("api/sales-order-header")]
    [ApiController]
    public class SalesOrderHeaderController : ControllerBase
    {
        private readonly ISalesOrderHeaderService _salesOrderHeaderService;

        public SalesOrderHeaderController(ISalesOrderHeaderService salesOrderHeaderService)
        {
            _salesOrderHeaderService = salesOrderHeaderService;
        }
        /// <summary>
        /// Gets sales orders depending on filter criteria.
        /// </summary>
        /// <returns>Sales orders with its respecttive sales details.</returns>
        /// <response code="200">Returns sales orders that meet filter criteria. It can return an empty array.</response>   
        /// <response code="400">If filter criteria parameters are not valid.</response>   
        [HttpGet]
        [ProducesResponseType(typeof(SalesReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [Route("search")]
        public async Task<IActionResult> SearchSalesOrders([FromQuery] PaginationParamsDto paginationParams, [FromQuery] SalesSearchtFiltersDto filterCriteria)
        {
            var salesOrdersDtos = await _salesOrderHeaderService.GetSalesReport(paginationParams, filterCriteria);

            return Ok(salesOrdersDtos);
        }

        /// <summary>
        /// Gets the top 25 product sales by region ordered percentage of sales in the region.
        /// </summary>
        /// <returns>Returns the top 25 product sales by region ordered percentage of sales in the region.</returns>
        /// <response code="200">Returns up to a maximum of 25 records that can cahnge over time.</response>   
        [HttpGet]
        [ProducesResponseType(typeof(TopProductSalesByRegionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [Route("report/top-by-region")]
        public async Task<IActionResult> GetTopSalesByRegionReport()
        {
            var topSalesDtos = await _salesOrderHeaderService.GetTopSalesByRegion();

            return Ok(topSalesDtos);
        }

    }
}

namespace RSMFinalProject.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RSMFinalProject.BLL.Services.Contract;
    using RSMFinalProject.DTO.Pagination;
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

        [HttpGet]
        [Route("/search")]
        public async Task<IActionResult> SearchSalesOrders([FromQuery] PaginationParamsDto paginationParams, [FromQuery] SalesSearchtFiltersDto filterCriteria)
        {
            var salesOrdersDtos = await _salesOrderHeaderService.GetSalesReport(paginationParams, filterCriteria);

            return Ok(salesOrdersDtos);
        }

        [HttpGet]
        [Route("/report/top-by-region")]
        public async Task<IActionResult> GetTopSalesByRegionReport([FromQuery] TopSalesByRegionReportFiltersDto filterCriteria)
        {
            var salesReportDtos = await _salesOrderHeaderService.GetTopSalesByRegionReport(filterCriteria);

            return Ok(salesReportDtos);
        }


        [HttpPost]
        [Route("")]
        public IActionResult Test([FromBody] SalesSearchtFiltersDto filterCriteria)
        {
            return Ok(":D");
        }
    }
}

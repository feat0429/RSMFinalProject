namespace RSMFinalProject.BLL.Services.Contract
{
    using RSMFinalProject.DTO.PagedList;
    using RSMFinalProject.DTO.Pagination;
    using RSMFinalProject.DTO.SalesOrderHeader;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;
    using System.Threading.Tasks;
    public interface ISalesOrderHeaderService
    {
        Task<PagedListDto<SalesReportDto>> GetSalesReport(PaginationParamsDto paginationParams, SalesSearchtFiltersDto filterCriteria);
        Task<IEnumerable<TopProductSalesByRegionDto>> GetTopSalesByRegionReport(TopSalesByRegionReportFiltersDto filterCriteria);
    }
}

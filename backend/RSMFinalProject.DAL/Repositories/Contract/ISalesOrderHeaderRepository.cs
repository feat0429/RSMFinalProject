
namespace RSMFinalProject.DAL.Repositories.Contract
{
    using RSMFinalProject.DAL.Pagination;
    using RSMFinalProject.DTO.Pagination;
    using RSMFinalProject.DTO.SalesOrderHeader;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;
    using RSMFinalProject.Model;

    public interface ISalesOrderHeaderRepository
    {
        Task<PagedList<SalesOrderHeader>> SearchSalesOrders(PaginationParamsDto paginationParams, SalesSearchtFiltersDto filterCriteria);
        Task<IEnumerable<TopProductSalesByRegionDto>> GetTopSalesByRegion();

    }
}

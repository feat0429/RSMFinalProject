namespace RSMFinalProject.DAL.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using RSMFinalProject.DAL.DbContext;
    using RSMFinalProject.DAL.Pagination;
    using RSMFinalProject.DAL.Repositories.Contract;
    using RSMFinalProject.DTO.Pagination;
    using RSMFinalProject.DTO.SalesOrderHeader;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;
    using RSMFinalProject.Model;

    public class SalesOrderHeaderRepository : ISalesOrderHeaderRepository
    {
        private readonly AdventureWorksContext _dbContext;

        public SalesOrderHeaderRepository(AdventureWorksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<SalesOrderHeader>> SearchSalesOrders(PaginationParamsDto paginationParams, SalesSearchtFiltersDto filterCriteria)
        {
            IQueryable<SalesOrderHeader> query = _dbContext.SalesOrderHeaders
                .AsNoTracking()
                .Include(salesOrder => salesOrder.SalesPerson)
                .Include(salesOrder => salesOrder.ShipToAddress)
                .Include(salesOrder => salesOrder.BillToAddress)
                .Include(salesOrder => salesOrder.Customer)
                    .ThenInclude(customer => customer.Person)
                .Include(salesOrder => salesOrder.SalesOrderDetails)
                    .ThenInclude(detail => detail.Product)
                        .ThenInclude(product => product.ProductSubcategory)
                            .ThenInclude(subcategory => subcategory!.ProductCategory);

            if (filterCriteria.StartDate is not null && filterCriteria.EndDate is null)
            {
                query = query.Where(salesOrder => salesOrder.OrderDate >= filterCriteria.StartDate);
            }

            if (filterCriteria.StartDate is null && filterCriteria.EndDate is not null)
            {
                query = query.Where(salesOrder => salesOrder.OrderDate <= filterCriteria.EndDate);
            }

            if (filterCriteria.StartDate is not null && filterCriteria.EndDate is not null)
            {
                query = query.Where(salesOrder =>
                salesOrder.OrderDate >= filterCriteria.StartDate
                && salesOrder.OrderDate <= filterCriteria.EndDate);

            }

            if (!string.IsNullOrWhiteSpace(filterCriteria.CustomerName))
            {
                query = query.Where(salesOrder => salesOrder.Customer.Person!.FirstName.Contains(filterCriteria.CustomerName)
                || salesOrder.Customer.Person!.LastName.Contains(filterCriteria.CustomerName));
            }

            if (!string.IsNullOrWhiteSpace(filterCriteria.SalesPersonName))
            {
                query = query.Where(salesOrder => salesOrder.SalesPerson!.FirstName.Contains(filterCriteria.SalesPersonName)
                || salesOrder.SalesPerson.LastName.Contains(filterCriteria.SalesPersonName));
            }

            if (!string.IsNullOrWhiteSpace(filterCriteria.ProductName))
            {
                query = query.Where(salesOrder => salesOrder.SalesOrderDetails
                    .Any(detail => detail.Product.Name.Contains(filterCriteria.ProductName)));
            }

            if (filterCriteria.ProductCategoryId is not null)
            {
                query = query.Where(salesOrder => salesOrder.SalesOrderDetails
                    .Any(detail => detail.Product.ProductSubcategory!.ProductCategory.ProductCategoryId == filterCriteria.ProductCategoryId));
            }

            return await PagedList<SalesOrderHeader>
                .CreateAsync(query, paginationParams.CurrentPage, paginationParams.PageSize);
        }

        public async Task<IEnumerable<TopProductSalesByRegionDto>> GetTopSalesByRegionReport(TopSalesByRegionReportFiltersDto filterCriteria)
        {

            return await _dbContext.Database
                .SqlQuery<TopProductSalesByRegionDto>($"EXECUTE dbo.uspGetTopSalesByRegionReport @ResultCount = {filterCriteria.ResultCount},@FilterDate = {filterCriteria.FilterDate},@ProductName = {filterCriteria.ProductName},@ProductCategoryID  = {filterCriteria.ProductCategoryId},@TerritoryID = {filterCriteria.TerritoryId};")
                .ToListAsync();
        }
    }
}

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
    using System.Runtime.CompilerServices;

    public class SalesOrderHeaderRepository : ISalesOrderHeaderRepository
    {
        private readonly AdventureWorksContext _dbContext;
        private readonly static string s_collation = "SQL_LATIN1_GENERAL_CP1_CI_AI";

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
                .Include(salesOrder => salesOrder.SalesTerritory)
                .Include(salesOrder => salesOrder.Customer)
                    .ThenInclude(customer => customer.Person)
                .Include(salesOrder => salesOrder.SalesOrderDetails.OrderBy(detail => detail.ProductId))
                    .ThenInclude(detail => detail.Product)
                        .ThenInclude(product => product.ProductSubcategory)
                            .ThenInclude(subcategory => subcategory!.ProductCategory)
                .OrderBy(s => s.SalesOrderId)
                .AsSplitQuery();

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
                query = query.Where(salesOrder => EF.Functions.Collate(
                    salesOrder.Customer.Person!.FirstName +
                    salesOrder.Customer.Person!.LastName, s_collation)
                    .Contains(filterCriteria.CustomerName));
            }

            if (!string.IsNullOrWhiteSpace(filterCriteria.SalesPersonName))
            {
                query = query.Where(salesOrder => EF.Functions.Collate(
                    salesOrder.SalesPerson!.FirstName +
                    salesOrder.SalesPerson.LastName, s_collation)
                    .Contains(filterCriteria.SalesPersonName));
            }

            if (!string.IsNullOrWhiteSpace(filterCriteria.SalesTerritoryId.ToString()))
            {
                query = query.Where(salesOrder => salesOrder.TerritoryId == filterCriteria.SalesTerritoryId);
            }

            return await PagedList<SalesOrderHeader>
                .CreateAsync(query, paginationParams.CurrentPage, paginationParams.PageSize);
        }

        public async Task<IEnumerable<TopProductSalesByRegionDto>> GetTopSalesByRegion()
        {
            var query = FormattableStringFactory.Create(
                @"DECLARE @FilterDate DATETIME;
                    SET @FilterDate = '2013-12-31 00:00:00.000';
                    WITH ProductInfo AS 
						(
							SELECT 
							p.ProductID
							,p.Name AS ProductName
							,pc.Name AS ProductCategory
							FROM Production.Product p
							INNER JOIN Production.ProductSubcategory ps
								ON p.ProductSubcategoryID = ps.ProductSubcategoryID
							INNER JOIN Production.ProductCategory pc
								ON ps.ProductCategoryID = pc.ProductCategoryID
						),
						SalesDate AS 
						(
							SELECT 
							soh.SalesOrderID
							,soh.OrderDate
							,YEAR(DATEADD(qq,-1,@FilterDate)) AS PreviousQuarterYear
							,DATEPART(qq,DATEADD(qq,-1,@FilterDate)) AS PreviousQuarter 
							,YEAR(@FilterDate) AS CurrentSalesYear
							,DATEPART(qq,@FilterDate) AS CurrentSalesQuarter
							FROM Sales.SalesOrderHeader soh
						),
						SalesInfo AS
						(
							SELECT
							soh.SalesOrderID
							,st.Name AS Territory
							,soh.OrderDate
							,sod.ProductID
							,SUM(sod.LineTotal) OVER(PARTITION BY pi.ProductID) AS TotalSales 	
							,SUM(sod.LineTotal) OVER(PARTITION BY soh.TerritoryID) AS TotalSalesByTerritory
							,SUM(sod.LineTotal) OVER(PARTITION BY soh.TerritoryID, pi.ProductID) AS ProductSalesByTerritory
							,SUM(sod.LineTotal) OVER(PARTITION BY soh.TerritoryID, pi.ProductCategory) AS CategorySalesByTerritory	
							,SUM(CASE 
									WHEN sd.CurrentSalesQuarter = DATEPART(qq,soh.OrderDate)
									AND sd.CurrentSalesYear = YEAR(soh.OrderDate)
									THEN sod.LineTotal 
								END) 
							OVER(PARTITION BY pi.ProductCategory) AS CurrentQuarterCategorySales 
							,SUM(CASE 
									WHEN sd.PreviousQuarter = DATEPART(qq,soh.OrderDate)
									AND sd.PreviousQuarterYear = YEAR(soh.OrderDate)
									THEN sod.LineTotal 
								END) 
							OVER(PARTITION BY pi.ProductCategory) AS PreviousQuarterCategorySales
							,SUM(CASE 
									WHEN sd.CurrentSalesQuarter = DATEPART(qq,soh.OrderDate)
									AND sd.CurrentSalesYear = YEAR(soh.OrderDate)
									THEN sod.LineTotal 
								END) 
							OVER(PARTITION BY soh.TerritoryID) AS CurrentQuarterTerritorySales 
							,SUM(CASE 
									WHEN sd.PreviousQuarter = DATEPART(qq,soh.OrderDate)
									AND sd.PreviousQuarterYear = YEAR(soh.OrderDate)
									THEN sod.LineTotal 
								END) 
							OVER(PARTITION BY soh.TerritoryID) AS PreviousQuarterTerritorySales
							FROM Sales.SalesOrderHeader soh
							INNER JOIN Sales.SalesOrderDetail sod
								ON soh.SalesOrderID = sod.SalesOrderID
							INNER JOIN Sales.SalesTerritory st
								ON soh.TerritoryID = st.TerritoryID
							INNER JOIN ProductInfo pi
								ON sod.ProductID = pi.ProductID
							INNER JOIN SalesDate sd
								ON sd.SalesOrderID = soh.SalesOrderID
						)
						SELECT DISTINCT TOP 10
						pi.ProductName
						,pi.ProductCategory
						,si.TotalSales
						,si.Territory
						,(si.ProductSalesByTerritory / si.TotalSalesByTerritory) AS SalesByRegion
						,(si.CategorySalesByTerritory / si.TotalSalesByTerritory) AS CategorySalesByRegion
						,CASE
							WHEN (si.CurrentQuarterCategorySales - si.PreviousQuarterCategorySales) > 5000
							THEN 'Growth'
							WHEN (si.CurrentQuarterCategorySales - si.PreviousQuarterCategorySales) < -5000
							THEN 'Decline'
							ELSE 'Normal' 
						END AS CategoryLastQuarter
						,CASE
							WHEN (si.CurrentQuarterTerritorySales - si.PreviousQuarterTerritorySales) > 5000
							THEN 'Growth'
							WHEN (si.CurrentQuarterTerritorySales - si.PreviousQuarterTerritorySales) < -5000
							THEN 'Decline'
							ELSE 'Normal' 
						END AS TerritoryLastQuarter
						FROM Sales.SalesOrderHeader soh
						INNER JOIN SalesInfo si
							ON soh.SalesOrderID = si.SalesOrderID
						INNER JOIN ProductInfo pi
							ON pi.ProductID = si.ProductID
						ORDER BY SalesByRegion DESC");


            return await _dbContext.Database
                .SqlQuery<TopProductSalesByRegionDto>(query)
                .ToListAsync();
        }
    }
}

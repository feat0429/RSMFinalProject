using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RSMFinalProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class GetTopSalesByRegionReportMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE OR ALTER PROCEDURE dbo.uspGetTopSalesByRegionReport
					@ResultCount INT = 10,
					@FilterDate DATETIME = NULL,
					@ProductName VARCHAR(50) = NULL,
					@ProductCategoryID INT = NULL,
					@TerritoryID INT = NULL
					AS
					BEGIN
						SET NOCOUNT ON;

						IF @FilterDate IS NULL
							SET @FilterDate = GETDATE();

						WITH ProductInfo AS 
						(
							SELECT 
							p.ProductID
							,p.Name AS ProductName
							,pc.Name AS ProductCategory
							,pc.ProductCategoryID
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
							,st.TerritoryID
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
							WHERE soh.OrderDate <= @FilterDate
						)
						SELECT DISTINCT TOP(@ResultCount)
						pi.ProductName
						,pi.ProductCategory
						,si.TotalSales
						,si.Territory
						,CAST((si.ProductSalesByTerritory / si.TotalSalesByTerritory) * 100 AS DECIMAL(10,2)) AS TotalSalesByRegionPercentage
						,CAST((si.CategorySalesByTerritory / si.TotalSalesByTerritory) * 100 AS DECIMAL(10,2)) AS TotalCategorySalesByRegionPercentage
						,CASE
							WHEN (si.CurrentQuarterCategorySales - si.PreviousQuarterCategorySales) > 5000
							THEN 'Growth'
							WHEN (si.CurrentQuarterCategorySales - si.PreviousQuarterCategorySales) < -5000
							THEN 'Decline'
							ELSE 'Normal' 
						END AS CategorySalesComparedWithPreviousQuarter
						,CASE
							WHEN (si.CurrentQuarterTerritorySales - si.PreviousQuarterTerritorySales) > 5000
							THEN 'Growth'
							WHEN (si.CurrentQuarterTerritorySales - si.PreviousQuarterTerritorySales) < -5000
							THEN 'Decline'
							ELSE 'Normal' 
						END AS TerritorySalesComparedWithPreviousQuarter
						FROM Sales.SalesOrderHeader soh
						INNER JOIN SalesInfo si
							ON soh.SalesOrderID = si.SalesOrderID
						INNER JOIN ProductInfo pi
							ON pi.ProductID = si.ProductID
						WHERE
							(pi.ProductName = '' OR @ProductName IS NULL OR pi.ProductName LIKE '%' + @ProductName + '%')
							AND
							(@ProductCategoryID IS NULL OR pi.ProductCategoryID = @ProductCategoryID)
							AND
							(@TerritoryID IS NULL OR si.TerritoryID = @TerritoryID)
						ORDER BY TotalSalesByRegionPercentage DESC;
					END;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.uspGetTopSalesByRegionReport");
        }
    }
}

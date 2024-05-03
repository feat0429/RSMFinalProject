namespace RSMFinalProject.DTO.SalesOrderHeader
{
    public record TopProductSalesByRegionDto
    (
        string ProductName,
        string ProductCategory,
        decimal TotalSales,
        string Territory,
        decimal SalesByRegion,
        decimal CategorySalesByRegion,
        string CategoryLastQuarter,
        string TerritoryLastQuarter
    );
}

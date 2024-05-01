namespace RSMFinalProject.DTO.SalesOrderHeader
{
    public record TopProductSalesByRegionDto
    (
        string ProductName,
        string ProductCategory,
        decimal TotalSales,
        string Territory,
        decimal TotalSalesByRegionPercentage,
        decimal TotalCategorySalesByRegionPercentage,
        string CategorySalesComparedWithPreviousQuarter,
        string TerritorySalesComparedWithPreviousQuarter
    );
}

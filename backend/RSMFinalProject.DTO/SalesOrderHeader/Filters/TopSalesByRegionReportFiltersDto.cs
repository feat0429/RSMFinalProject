namespace RSMFinalProject.DTO.SalesOrderHeader.Filters
{
    public record TopSalesByRegionReportFiltersDto
    (
        DateTime FilterDate,
        string? ProductName,
        int? ProductCategoryId,
        int? TerritoryId
    );
}

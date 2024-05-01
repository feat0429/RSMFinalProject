namespace RSMFinalProject.DTO.SalesOrderHeader.Filters
{
    public record TopSalesByRegionReportFiltersDto
    (
        int ResultCount,
        DateTime? FilterDate,
        string? ProductName,
        int? ProductCategoryId,
        int? TerritoryId
    );
}

namespace RSMFinalProject.DTO.SalesOrderHeader.Filters
{
    public record SalesSearchtFiltersDto
    (
        DateTime? StartDate,
        DateTime? EndDate,
        string? CustomerName,
        string? SalesPersonName,
        string? ProductName,
        int? ProductCategoryId
    );
}

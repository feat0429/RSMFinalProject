namespace RSMFinalProject.DTO.SalesOrderDetail
{
    public record SalesOrderDetailReportDto
    (
        string ProductName,
        string ProductCategory,
        decimal UnitPrice,
        int Quantity,
        decimal LineTotal
    );
}

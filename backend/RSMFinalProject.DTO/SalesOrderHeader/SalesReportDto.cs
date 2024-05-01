using RSMFinalProject.DTO.SalesOrderDetail;

namespace RSMFinalProject.DTO.SalesOrderHeader
{
    public record SalesReportDto
    (
        DateTime OrderDate,
        string CustomerName,
        string SalesPersonName,
        string ShippingAddress,
        string BillingAddress,
        decimal SubTotal,
        decimal TotalDue,
        IEnumerable<SalesOrderDetailReportDto> SalesOrderDetails
    );
}

namespace RSMFinalProject.BLL.Mappers
{
    using AutoMapper;
    using RSMFinalProject.DTO.SalesOrderDetail;
    using RSMFinalProject.Model;

    public class SalesOrderDetailProfile : Profile
    {
        public SalesOrderDetailProfile()
        {
            CreateMap<SalesOrderDetail, SalesOrderDetailReportDto>()
                .ForCtorParam(nameof(SalesOrderDetailReportDto.ProductName),
                    opt => opt.MapFrom(source => source.Product.Name))
                .ForCtorParam(nameof(SalesOrderDetailReportDto.ProductCategory),
                    opt => opt.MapFrom(source => source.Product.ProductSubcategory!.ProductCategory.Name))
                .ForCtorParam(nameof(SalesOrderDetailReportDto.UnitPrice),
                    opt => opt.MapFrom(source => source.UnitPrice))
                .ForCtorParam(nameof(SalesOrderDetailReportDto.Quantity),
                    opt => opt.MapFrom(source => source.OrderQty));
        }
    }
}

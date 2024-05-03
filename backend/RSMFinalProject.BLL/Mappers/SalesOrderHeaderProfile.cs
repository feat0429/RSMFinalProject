namespace RSMFinalProject.BLL.Mappers
{
    using AutoMapper;
    using RSMFinalProject.DAL.Pagination;
    using RSMFinalProject.DTO.Pagination;
    using RSMFinalProject.DTO.SalesOrderHeader;
    using RSMFinalProject.Model;
    public class SalesOrderHeaderProfile : Profile
    {
        public SalesOrderHeaderProfile()
        {
            CreateMap<SalesOrderHeader, SalesReportDto>()
            .ForCtorParam(nameof(SalesReportDto.OrderDate),
                opt => opt.MapFrom(source => source.OrderDate))
            .ForCtorParam(nameof(SalesReportDto.CustomerName),
                opt => opt.MapFrom(source => source.Customer.Person!.FirstName + " " + source.Customer.Person.LastName))
            .ForCtorParam(nameof(SalesReportDto.SalesPersonName),
                opt => opt.MapFrom(source => source.SalesPerson!.FirstName + " " + source.SalesPerson.LastName))
            .ForCtorParam(nameof(SalesReportDto.SalesOrderDetails),
                opt => opt.MapFrom(source => source.SalesOrderDetails))
            .ForCtorParam(nameof(SalesReportDto.ShippingAddress),
                opt => opt.MapFrom(source => source.ShipToAddress.AddressLine1))
            .ForCtorParam(nameof(SalesReportDto.BillingAddress),
                opt => opt.MapFrom(source => source.BillToAddress.AddressLine1))
            .ForCtorParam(nameof(SalesReportDto.SalesTerritory),
                opt => opt.MapFrom(source => source.SalesTerritory.Name));

            CreateMap<PagedList<SalesOrderHeader>, PagedListDto<SalesReportDto>>();
            CreateMap<PagedList<TopProductSalesByRegionDto>, PagedListDto<TopProductSalesByRegionDto>>();

        }
    }
}

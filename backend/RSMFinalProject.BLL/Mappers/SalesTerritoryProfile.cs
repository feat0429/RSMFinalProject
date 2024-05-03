namespace RSMFinalProject.BLL.Mappers
{
    using AutoMapper;
    using RSMFinalProject.DTO.Territory;
    using RSMFinalProject.Model;

    public class SalesTerritoryProfile : Profile
    {
        public SalesTerritoryProfile()
        {
            CreateMap<SalesTerritory, GetAllSalesTerritoriesDto>();
        }

    }
}

namespace RSMFinalProject.BLL.Services
{
    using AutoMapper;
    using RSMFinalProject.BLL.Services.Contract;
    using RSMFinalProject.DAL.Repositories.Contract;
    using RSMFinalProject.DTO.Territory;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SalesTerritoryService : ISalesTerritoryService
    {
        private readonly ISalesTerritoryRepository _salesTerritoryRepository;
        private readonly IMapper _mapper;

        public SalesTerritoryService(ISalesTerritoryRepository salesTerritoryRepository, IMapper mapper)
        {
            _salesTerritoryRepository = salesTerritoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllSalesTerritoriesDto>> GetAllSalesTerritories()
        {
            var salesTerritories = await _salesTerritoryRepository.GetAllSalesTerritories();

            return _mapper.Map<List<GetAllSalesTerritoriesDto>>(salesTerritories);
        }
    }
}

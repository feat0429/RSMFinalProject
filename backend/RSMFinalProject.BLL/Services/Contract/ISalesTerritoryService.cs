namespace RSMFinalProject.BLL.Services.Contract
{
    using RSMFinalProject.DTO.Territory;

    public interface ISalesTerritoryService
    {
        Task<IEnumerable<GetAllSalesTerritoriesDto>> GetAllSalesTerritories();
    }
}

namespace RSMFinalProject.DAL.Repositories.Contract
{
    using RSMFinalProject.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISalesTerritoryRepository
    {
        Task<IEnumerable<SalesTerritory>> GetAllSalesTerritories();
    }
}

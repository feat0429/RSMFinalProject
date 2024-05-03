namespace RSMFinalProject.DAL.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using RSMFinalProject.DAL.DbContext;
    using RSMFinalProject.DAL.Repositories.Contract;
    using RSMFinalProject.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class SalesTerritoryRepository : ISalesTerritoryRepository
    {
        private readonly AdventureWorksContext _dbContext;

        public SalesTerritoryRepository(AdventureWorksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SalesTerritory>> GetAllSalesTerritories()
        {
            return await _dbContext.SalesTerritories
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

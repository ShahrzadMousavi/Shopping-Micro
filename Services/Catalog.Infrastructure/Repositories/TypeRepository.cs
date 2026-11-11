using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    internal class TypeRepository(ICatalogContext context) : ITypeRepository
    {
        public async Task<IEnumerable<ProductType>> GetProductTypes()
        {
            return await context.Types.Find(p => true).ToListAsync();
        }
    }
}

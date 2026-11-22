using Catalog.Core.CatalogSpecs;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository(ICatalogContext context) : IProductRepository
    {
        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams)
        {
            var builder = Builders<Product>.Filter;//Create Filter
            var filters = builder.Empty;//Empty
            //Filters
            filters = BuilderFilterDefinition(specParams, builder, filters);
            //Get Total Items
            long totalItems = await GetTotalItems(filters);
            //Sort Data
            SortDefinition<Product> sort = SortDefinition(specParams);
            List<Product> data = await GetData(specParams, filters, sort);
            return new Pagination<Product>(specParams.PageIndex, specParams.PageSize, (int)totalItems, data);
        }

        private async Task<List<Product>> GetData(CatalogSpecParams specParams, FilterDefinition<Product> filters, SortDefinition<Product> sort)
        {
            return await context.Products.Find(filters)
                .Sort(sort)
                .Skip((specParams.PageIndex - 1) * specParams.PageSize)
                .Limit(specParams.PageSize)
                .ToListAsync();
        }

        private static SortDefinition<Product> SortDefinition(CatalogSpecParams specParams)
        {
            var sort = Builders<Product>.Sort.Ascending(x => x.Name);//Default sort by name
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                sort = specParams.Sort switch
                {
                    "priceAsc" => Builders<Product>.Sort.Ascending(x => x.Price),
                    "priceDesc" => Builders<Product>.Sort.Descending(x => x.Price),
                    _ => Builders<Product>.Sort.Ascending(x => x.Name),//Default sort by name
                };
            }

            return sort;
        }

        private async Task<long> GetTotalItems(FilterDefinition<Product> filters)
        {
            return await context.Products.CountDocumentsAsync(filters);
        }

        private static FilterDefinition<Product> BuilderFilterDefinition(CatalogSpecParams specParams, FilterDefinitionBuilder<Product> builder, FilterDefinition<Product> filters)
        {
            if (!string.IsNullOrEmpty(specParams.Search))
            {
                var searchFilter= builder.Where(x => x.Name.ToLower().Contains(specParams.Search.ToLower()));
                filters &= searchFilter;
            }
            if (!string.IsNullOrEmpty(specParams.BrandId))
            {
                var brandFilter = builder.Eq(x => x.Brands.Id, specParams.BrandId);
                filters &= brandFilter;
            }
            if (!string.IsNullOrEmpty(specParams.TypeId))
            {
                var typeFilter = builder.Eq(x => x.Types.Id, specParams.TypeId);
                filters &= typeFilter;
            }

            return filters;
        }

        public async Task<Product> GetProductById(string id)
        {
            return await context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            return await context.Products.Find(p => p.Name == name).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByType(string type)
        {
            return await context.Products.Find(p => p.Types.Name == type).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByBrand(string brand)
        {
            return await context.Products.Find(p => p.Brands.Name == brand).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByBrandId(string brandId)
        {
            return await context.Products.Find(p => p.Brands.Id == brandId).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByTypeId(string typeId)
        {
            return await context.Products.Find(p => p.Types.Id == typeId).ToListAsync();
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            var result= await context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteProduct(string id)
        {
            var result = await context.Products.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            return await DeleteProduct(product.Id);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await context.Products.InsertOneAsync(product);
            return product;
        }
    }
}

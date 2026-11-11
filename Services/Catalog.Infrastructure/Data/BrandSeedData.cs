using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class BrandSeedData
    {
        public static void SeedData(IMongoCollection<ProductBrand> collection)
        {
            var existCollection= collection.Find(p => true).Any();
            if (!existCollection)
            {
                return;
            }
            var pathJson=Path.Combine(AppContext.BaseDirectory, "Data", "SeedData","brands.json");
            if (!File.Exists(pathJson))
            {
                throw new Exception($"File {pathJson} not found");
            }
            var dataText=File.ReadAllText(pathJson);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(dataText);
            if (brands is not null && brands.Any())
            {
                collection.InsertMany(brands);
            }
        }
    }
}

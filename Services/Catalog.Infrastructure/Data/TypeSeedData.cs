using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class TypeSeedData
    {
        public static void SeedData(IMongoCollection<ProductType> collection)
        {
            var existCollection = collection.Find(p => true).Any();
            if (!existCollection)
            {
                return;
            }
            var pathJson = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "types.json");
            if (!File.Exists(pathJson))
            {
                throw new Exception($"File {pathJson} not found");
            }
            var dataText = File.ReadAllText(pathJson);
            var types = JsonSerializer.Deserialize<List<ProductType>>(dataText);
            if (types is not null && types.Any())
            {
                collection.InsertMany(types);
            }
        }
    }
}

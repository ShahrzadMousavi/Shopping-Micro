using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public static class ProductSeedData
    {
        public static void SeedData(IMongoCollection<Product> collection)
        {
            var existCollection = collection.Find(p => true).Any();
            if (!existCollection)
            {
                return;
            }
            var pathJson = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "products.json");
            if (!File.Exists(pathJson))
            {
                throw new Exception($"File {pathJson} not found");
            }
            var dataText = File.ReadAllText(pathJson);
            var products = JsonSerializer.Deserialize<List<Product>>(dataText);
            if (products is not null && products.Any())
            {
                collection.InsertMany(products);
            }
        }
    }
}

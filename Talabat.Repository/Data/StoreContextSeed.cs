using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {

                var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (Brands is not null && Brands.Count > 0)
                {
                    foreach (var brand in Brands)
                    {
                        await context.Set<ProductBrand>().AddAsync(brand);

                    }
                    await context.SaveChangesAsync();
                }
            }
            if (!context.ProductTypes.Any())
            {

                var TypeData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);

                if (Types is not null && Types.Count > 0)
                {
                    foreach (var type in Types)
                    {
                        await context.Set<ProductType>().AddAsync(type);

                    }
                    await context.SaveChangesAsync();
                }
            }
            if (!context.Products.Any())
            {

                var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (Products is not null && Products.Count > 0)
                {
                    foreach (var product in Products)
                    {
                        await context.Set<Product>().AddAsync(product);

                    }
                    await context.SaveChangesAsync();
                }
            }
            if (!context.DeliveryMethods.Any())
            {

                var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods is not null && deliveryMethods.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        await context.Set<DeliveryMethod>().AddAsync(deliveryMethod);

                    }
                    await context.SaveChangesAsync();
                }
            }

        }
    }
}

using Agency.CustomerModule.Models;
using Core.Base;

namespace Agency.CustomerModule.Services;

public class ProductExternal : ServiceBase, IProductExternal

{
    public Product? GetProduct(string productId) => GetData<Product>("https://dummyjson.com/products/" + productId);

    public Task<Product?> GetProductAsync(string productId) => Task.FromResult(GetProduct(productId));
}
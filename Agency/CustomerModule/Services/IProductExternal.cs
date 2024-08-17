using Agency.CustomerModule.Models;

namespace Agency.CustomerModule.Services;

public interface IProductExternal
{
    Product? GetProduct(string productId);
    Task<Product?> GetProductAsync(string productId);
}
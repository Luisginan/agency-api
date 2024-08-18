using AgencyApi.CustomerModule.Models;

namespace AgencyApi.CustomerModule.Services;

public interface IProductExternal
{
    Product? GetProduct(string productId);
    Task<Product?> GetProductAsync(string productId);
}
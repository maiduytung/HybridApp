using BlazorShared.Models;

namespace BlazorShared.Services
{
    public interface IProductService
    {
        Task<bool> AddProduct(Product product);
        Task<List<Product>> GetProductList();
        Task<Product> GetProduct(string barcodeText);
    }
}
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProductAsync(ProductRequest product);
        Task<List<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(int id);
        Task<ProductResponse?> UpdateProductAsync(int id, ProductRequest updatedProduct);
        Task<ProductResponse?> DeleteProductAsync(int id);
        Task<ProductResponse?> DecrementStockAsync(int id, int quantity);
        Task<ProductResponse?> AddToStockAsync(int id, int quantity);
    }
}

using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models;
using ProductsAPI.Validator;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly ProductRequestValidator _validator;
        private readonly IConfiguration _appSettings;
        private readonly int maxStock;

        public ProductService(AppDbContext context, ProductRequestValidator validator, IConfiguration appSettings)
        {
            _context = context;
            _validator = validator;
            _appSettings = appSettings;
            maxStock = Convert.ToInt32(string.IsNullOrEmpty(_appSettings["MaxStock"]) ? "1000" : _appSettings["MaxStock"]);
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
        {
            _validator.ValidateProduct(request);

            if (request.Name == null || request.Price == null || request.Description == null)
            {
                throw new ArgumentException("Required field(s) is/are missing, please check the request JSON");
            }

            var product = new Products
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price.Value,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var response = ResponseJSON(product);
            response.Message = "Product created successfully.";
            response.Success = true;
            return response;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            return products.Select(p => new ProductResponse
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockAvailable = p.StockAvailable,
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? null : ResponseJSON(product);
        }

        public async Task<ProductResponse?> UpdateProductAsync(int id, ProductRequest request)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
            {
                return null;
            }

            _validator.ValidateProductUpdate(request);

            bool isModified = false;

            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != existing.Name)
            {
                existing.Name = request.Name;
                isModified = true;
            }
            if (!string.IsNullOrWhiteSpace(request.Description) && request.Description != existing.Description)
            {
                existing.Description = request.Description;
                isModified = true;
            }
            if (request.Price.HasValue && request.Price.Value > 0 && request.Price.Value != existing.Price)
            {
                existing.Price = request.Price.Value;
                isModified = true;
            }
            if (isModified)
            {
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var response = ResponseJSON(existing);
                response.Success = true;
                response.Message = "Product updated successfully.";
                return response;
            }
            else
            {
                return new ProductResponse{ Success = false, Message = "No new values were provided. Product information remains unchanged"};
            }
        }

        public async Task<ProductResponse?> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            var response = ResponseJSON(product);
            response.Message = "Product deleted successfully.";
            response.Success = true;

            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ProductResponse?> AddToStockAsync(int id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            if (product.StockAvailable + quantity > maxStock)
                throw new InvalidOperationException($"Stock limit exceeded. Maximum allowed is {maxStock}.");

            product.StockAvailable += quantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ResponseJSON(product);
        }

        public async Task<ProductResponse?> DecrementStockAsync(int id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            if (quantity > product.StockAvailable)
                throw new InvalidOperationException("Cannot decrement beyond available stock.");

            product.StockAvailable -= quantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ResponseJSON(product);
        }

        private static ProductResponse ResponseJSON(Products product)
        {
            return new ProductResponse
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockAvailable = product.StockAvailable,
                CreatedAt = product.CreatedAt
            };
        }
    }
}
using ProductsAPI.Models;

namespace ProductsAPI.Validator
{
    public class ProductRequestValidator
    {
        public void ValidateProduct(ProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Product name cannot be empty");

            if (request.Name.Length > 100)
                throw new ArgumentException("Product name must not exceed 100 characters");

            if (string.IsNullOrWhiteSpace(request.Description))
                throw new ArgumentException("Product description cannot be empty");

            if (request.Description.Length > 500)
                throw new ArgumentException("Product description must not exceed 500 characters");

            if (request.Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.Price), "Price must be non-negative and greater than Zero");
        }
        public void ValidateProductUpdate(ProductRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name.Length > 100)
                throw new ArgumentException("Product name must not exceed 100 characters");

            if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Length > 500)
                throw new ArgumentException("Product description must not exceed 500 characters");

            if (request.Price.HasValue && request.Price.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.Price), "Price must be greater than zero");
        }
    }
}


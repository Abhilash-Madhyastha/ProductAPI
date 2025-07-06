using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using ProductsAPI.Services;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<ProductsController> _logger;
        private readonly string ProductNotFoundError;
        private readonly string GenericError;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger, IConfiguration appSettings)
        {
            _productService = productService;
            _logger = logger;
            _appSettings = appSettings;
            ProductNotFoundError = _appSettings.GetSection("ErrorMessages").GetSection("ProductNotFoundError").Value ?? "Product ID not found.";
            GenericError = _appSettings.GetSection("ErrorMessages").GetSection("GenericError").Value ?? "An unexpected error occurred. Please try again later.";
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest product)
        {
            try
            {
                var created = await _productService.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = created.ProductId }, created);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Error creating product");
                return BadRequest(new ProductResponse{ Success = false, Message = ex.Message  });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error creating product");
                return BadRequest(new ProductResponse { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating the product");
                return StatusCode(500, new ProductResponse { Success = false, Message = GenericError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                if (products == null || products.Count == 0)
                {
                    return NotFound(new ProductResponse { Success = true, Message = "No products found." });
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected Error while retrieving products");
                return StatusCode(500, new ProductResponse { Success = false, Message = GenericError });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ProductResponse { Success = false, Message = "Invalid Product ID" });
            }
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return product == null ? NotFound(new ProductResponse { ProductId = id, Success = false, Message = ProductNotFoundError }) : Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected Error while retrieving product with ID: {Id}", id);
                return StatusCode(500, new ProductResponse { Success = false, Message = GenericError });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest updatedProduct)
        {
            try
            {
                var product = await _productService.UpdateProductAsync(id, updatedProduct);
                _logger.LogInformation("Attempting to update product with ID: {Id}", id);
                return product == null ? NotFound(new ProductResponse { ProductId = id, Success = false, Message = ProductNotFoundError }) : Ok(product);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ProductResponse { Success = false, Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new  ProductResponse { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating product with ID: {Id}", id);
                return StatusCode(500, new ProductResponse { Success = false, Message = GenericError });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ProductResponse { Success = false, Message= "Invalid product ID." });
            }
            try
            {
                _logger.LogInformation("Attempting to delete product with ID: {Id}", id);
                var result = await _productService.DeleteProductAsync(id);
                return result == null ? NotFound(new ProductResponse { ProductId = id, Success = false, Message = ProductNotFoundError }) : Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting product with ID: {Id}", id);
                return StatusCode(500, new ProductResponse { Success = false, Message = GenericError });
            }
        }

        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStock(int id, int quantity)
        {
            if (id <= 0)
            {
                return BadRequest(new ProductResponse { Success = false, Message = "Invalid Id" });
            }

            if (quantity < 0)
            {
                return BadRequest(new ProductResponse { Success = false, Message = "Quantity must be greater than zero to decrement" });
            }
            try
            {
                _logger.LogInformation("Attempting to decrement stock for product ID: {Id} by quantity: {Quantity}", id, quantity);
                var product = await _productService.DecrementStockAsync(id, quantity);
                return product == null ? NotFound(new ProductResponse { ProductId = id, Success = false, Message = ProductNotFoundError }) : Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ProductResponse { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while decrementing stock for product ID: {Id}", id);
                return StatusCode(500, new ProductResponse { Success = false, Message = GenericError });
            }
        }

        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> AddToStock(int id, int quantity)
        {
            if (id <= 0 )
            {
                return BadRequest(new ProductResponse { Success = false, Message = "Invalid Id" });
            }
            if (quantity <= 0)
            {
                return BadRequest(new ProductResponse { Success = false, Message = "Quantity must be greater than zero to add" });
            }
            try
            {  
                _logger.LogInformation("Attempting to add stock for product ID: {Id} by quantity: {Quantity}", id, quantity);
                var product = await _productService.AddToStockAsync(id, quantity);
                return product == null ? NotFound(new ProductResponse { ProductId = id, Success = false, Message = ProductNotFoundError }) : Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ProductResponse { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding stock for product ID: {Id}", id);
                return StatusCode(500, new ProductResponse { Success = false, Message = GenericError });
            }
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models.Response;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRefactorController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductRefactorController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(new ApiResponse<IEnumerable<Product>>
            {
                StatusCode = StatusCodes.Status200OK,
                Data = products,
                Message = "Products retrieved successfully."
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound(new ApiResponse<Product>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Product not found."
                });
            }

            return Ok(new ApiResponse<Product>
            {
                StatusCode = StatusCodes.Status200OK,
                Data = product,
                Message = "Product retrieved successfully."
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Product>>> CreateProduct(Product product)
        {
            var newProductId = await _productService.CreateProduct(product);
            product.Id = newProductId;

            return CreatedAtAction(nameof(GetProduct), new { id = newProductId }, new ApiResponse<Product>
            {
                StatusCode = StatusCodes.Status201Created,
                Data = product,
                Message = "Product created successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteProduct(int id)
        {
            var deleteProduct = await _productService.DeleteProduct(id);
            if (!deleteProduct)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Product not found."
                });
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null,
                    Message = "Product ID mismatch."
                });
            }

            var updateProduct = await _productService.UpdateProduct(product);
            if (!updateProduct)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = null,
                    Message = "Product not found."
                });
            }

            return NoContent();
        }

    }
}

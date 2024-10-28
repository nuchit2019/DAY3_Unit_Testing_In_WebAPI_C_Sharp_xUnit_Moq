using Moq;
using ProductAPI.Models;
using ProductAPI.Repositories;
using ProductAPI.Services;

namespace TestProductAPI.Services
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mockProducyRepo;
        private readonly ProductService _productService;
        public ProductServiceTest()
        {
            _mockProducyRepo = new Mock<IProductRepository> ();
            _productService = new ProductService(_mockProducyRepo.Object);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnProductId_whenProductIsCreated()
        {
            // Arrange
            int prodictId = 1;
            var prodcut = new Product
            {
                Name = "Product A",
                Price = 10
            };

            _mockProducyRepo
                .Setup(r => r.CreateProduct(prodcut))
                .ReturnsAsync(prodictId);

            // Act
            var result = await _productService.CreateProduct(prodcut);

            // Assert
            Assert.Equal(prodictId, result);

        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnTrue_whenProductIsDelete()
        {
            // Arrange
            int prodictId = 1;
             

            _mockProducyRepo
                .Setup(r => r.DeleteProduct(prodictId))
                .ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteProduct(prodictId);

            // Assert
            Assert.True(result);

        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_whenProductExists()
        {
            // Arrange
            int prodictId = 1;
            var product = new Product
            {
                Id = prodictId,
                Name = "Product A",
                Price = 10
            };


            _mockProducyRepo
                .Setup(r => r.GetProductById(prodictId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductById(prodictId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);

        }


        //TODO ... GetProducts
        [Fact]

        public async Task GetProducts_ShouldReturnListofProduct()
        {
            // Arrange
            
            var productList = new List<Product>
            {
               new Product{ Id = 1,  Name = "Product A", Price = 10},
               new Product{ Id = 2,  Name = "Product B", Price = 20},
            };


            _mockProducyRepo
                .Setup(r => r.GetProducts())
                .ReturnsAsync(productList);

            // Act
            var result = await _productService.GetProducts();

            // Assert
            Assert.NotNull(result);             
            Assert.Equal(2, ((List<Product>)result).Count ());
        }

        //TODO ...UpdateProduct
        [Fact]
        public async Task UpdateProduct_ShouldReturnTrue_WhenProductIsUpdated()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product A",
                Price = 10
            };


            _mockProducyRepo
                .Setup(r => r.UpdateProduct(product))
                .ReturnsAsync(true);

            // Act
            var result = await _productService.UpdateProduct(product);

            // Assert
            Assert.True(result);
           
        }

    }
}

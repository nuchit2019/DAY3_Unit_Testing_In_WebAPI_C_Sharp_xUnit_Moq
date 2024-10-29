using Moq;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Repositories;
using System.Data;

namespace TestProductAPI.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly Mock<IDapperContext>  _mockContext;
        private readonly Mock<IDbConnection> _mockDbConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrap;

        private readonly ProductRepository _productRepository;

        public ProductRepositoryTest()
        {
            _mockContext = new Mock<IDapperContext>();
            _mockDbConnection = new Mock<IDbConnection>();

            _mockDapperWrap = new Mock<IDapperWrapper>();

            _mockContext
                .Setup(x => x.CreateConnection())
                .Returns(_mockDbConnection.Object);

            _productRepository = new ProductRepository(_mockContext.Object, _mockDapperWrap.Object);

        }

        [Fact (DisplayName = "เพิ่ม ข้อมูล สินค้า")]
        public async Task CreateProduct_ShouldRetuenNewProductId()
        {
            // Arrange
            var product = new Product { Name = "Product A", Price = 10 };

            //string sql = "Insert ...";
            _mockDapperWrap
                .Setup(p => p.ExecuteScalarAsync<int>(_mockDbConnection.Object, It.IsAny<string>(), product))
                .ReturnsAsync(1);

            // Act
            var result = await _productRepository.CreateProduct(product);

            // Assert
            Assert.Equal(1, result);

        }


        [Fact(DisplayName = "ลบข้อมูล สินค้า")]
        public async Task DeleteProduct_ShouldReturnTrue_whenDelete()
        {
            // Arrange
            int productId = 1;

            _mockDapperWrap
                .Setup(c => c.ExecuteAsync(_mockDbConnection.Object, It.IsAny<string>(),It.IsAny<object>()))
                .ReturnsAsync(1);
             

            // Act
            var result = await _productRepository.DeleteProduct(productId);


            // Assert
            Assert.True(result);


        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenExixts()
        {
            // Arrange
            int productId = 1;
            var expectedProduct = new Product
            {
                Id = productId,
                Name = "Product A",
                Price = 10
            };
            _mockDapperWrap
                .Setup(c => c.QueryFirstOrDefaultAsync<Product>(_mockDbConnection.Object, It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productRepository.GetProductById(productId);


            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Id, result.Id);
            Assert.Equal(expectedProduct.Name, result.Name);
            Assert.Equal(expectedProduct.Price, result.Price);

        }
    
        [Fact]
        public async Task GetProducts_ShouldReturnListOfProduct()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "ProductA", Price = 10 },
                 new Product { Id = 2, Name = "ProductB", Price = 20 }
            };

            _mockDapperWrap
                .Setup(c => c.QueryAsync<Product>(_mockDbConnection.Object, It.IsAny<string>(), null))
                .ReturnsAsync(products);
            
            // Act
            var result = await _productRepository.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Product>)result).Count());
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnTrue_WhenUpdated()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product A", Price = 10 };
            _mockDapperWrap.Setup(s => s.ExecuteAsync(_mockDbConnection.Object, It.IsAny<string>(), product))
                .ReturnsAsync(1);

            // Act
            var result = await _productRepository.UpdateProduct(product);


            // Assert
            Assert.True(result);

        }
    }
}
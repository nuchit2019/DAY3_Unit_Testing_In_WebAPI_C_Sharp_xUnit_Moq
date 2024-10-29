using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using ProductAPI;
using ProductAPI.Data;

using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Models;
using System.Data;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net;

namespace TestProductAPI.Integration
{
    public class ProductAPI_integrationTest_mock_db : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IDapperWrapper> _dapperMock;
        private readonly IFixture _fixture;
        private readonly JsonSerializerOptions _options;

        public ProductAPI_integrationTest_mock_db(WebApplicationFactory<Program> factory)
        {
            _dapperMock = new Mock<IDapperWrapper>();
            var appFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the actual DapperWrapper with the mock
                    services.AddSingleton<IDapperWrapper>(_dapperMock.Object);
                });
            });

            _client = appFactory.CreateClient();

            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        }

        [Fact]
        public async Task GetProduct_ReturnOk_ListOfProduct()
        {
            // Arrange
            var productlist = _fixture.Build<Product>()
                .With(p=>p.Name, () => new string ("Product A "+new Random().Next(1,100)))
                .With(p=>p.Price,()=>new decimal(new Random().NextDouble()*100*0.01))
                .CreateMany(5)
                .ToList();

            _dapperMock
                .Setup(db => db.QueryAsync<Product>(It.IsAny<IDbConnection>(), It.IsAny<string>(), null))
                .ReturnsAsync(productlist);


            // Act
            var response = await _client.GetAsync("/api/product");


            // Assert
            response.EnsureSuccessStatusCode();
            var productList = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.NotNull(productList);
            Assert.Equal(5, productList.Count());

        }


        // GetProductById
        [Fact]
        public async Task GetProduct_WithExiststing_Return_Product()
        {
            // Arrange
            var producId = 1;
            var product = _fixture.Build<Product>()
                .With(p => p.Id, producId)
                .With(p => p.Name, "Keyboard")
                .With(p => p.Price, 150)
                .Create();

            _dapperMock
              .Setup(db => db.QueryFirstOrDefaultAsync<Product>(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
              .ReturnsAsync(product);


            // Act
            var response = await _client.GetAsync($"/api/product/{producId}");

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            var resProduct = JsonSerializer.Deserialize<Product>(responseString);
            //response.EnsureSuccessStatusCode();
            Assert.NotNull(resProduct);


        }

        //======================================================================//
        //TODO ... 
        [Fact]
        public async Task GetProduct_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            int productId = 1;
            _dapperMock
                .Setup(db => db.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(null);

            // Act
            var response = await _client.GetAsync($"/api/product/{productId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

 
        [Fact]
        public async Task CreateProduct_ReturnsCreatedAtActionAndProduct()
        {
            // Arrange
            var newProduct = new Product { Name = "New Mock Product", Price = 15 };
            // Mocking ExecuteAsync for CreateProduct
            _dapperMock
                .Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(1); // Return 1 row affected

            // Act
            var response = await _client.PostAsJsonAsync("/api/product", newProduct);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
 
        [Fact]
        public async Task DeleteProduct_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            int productId = 1;
            // Mocking ExecuteAsync for DeleteProduct
            _dapperMock
                .Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(1);

            // Act
            var response = await _client.DeleteAsync($"/api/product/{productId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var productId = 999; // Assume this ID does not exist
            _dapperMock
                .Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(0); // Return 0 rows affected to simulate non-existing product

            // Act
            var response = await _client.DeleteAsync($"/api/product/{productId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenIdDoesNotMatchProductId()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = 2, Name = "Updated Product", Price = 20m }; // Different ID

            //_dapperMock
            //   .Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
            //   .ReturnsAsync(0); // Simulate product not found

            // Act
            var response = await _client.PutAsJsonAsync($"/api/product/{productId}", product);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Updated Product", Price = 20m };

            _dapperMock
                .Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(0); // Simulate product not found

            // Act
            var response = await _client.PutAsJsonAsync($"/api/product/{productId}", product);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent_WhenProductIsUpdated()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Updated Product", Price = 20m };

            _dapperMock
                .Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(1); // Simulate successful update

            // Act
            var response = await _client.PutAsJsonAsync($"/api/product/{productId}", product);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
         


    }
}

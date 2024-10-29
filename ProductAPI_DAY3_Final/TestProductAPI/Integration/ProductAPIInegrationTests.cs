using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductAPI;
using ProductAPI.Controllers;
using ProductAPI.Models;
using System.Net;
using System.Net.Http.Json;

namespace TestProductAPI.Integration
{
    public class ProductAPIInegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public ProductAPIInegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();


            Console.WriteLine("BaseAddress: " + factory.Server.BaseAddress);
            Console.WriteLine("Port: "+factory.Server.BaseAddress.Port);
        }

        //https://localhost:7043/api/Product
        [Fact]
        public async Task GetProcudt_ReturnOkAndListOfProducts()
        {
            // Arrange


            // Act
            var response = await _client.GetAsync("/api/Product");


            // Assert
            //respone.Ensu
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadFromJsonAsync<List<Product>>();
            
             Assert.NotNull(product);
        }

        //https://localhost:7043/api/Product/5
        [Fact]
        public async Task GetProduct_WithExistingId_ReturnOKProduct()
        {
            // Arrange
            var productId = 5;

            // Act
            var res = await _client.GetAsync($"/api/Product/{productId}");


            // Assert
            res.EnsureSuccessStatusCode();
            var product = await res.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(product);
        }

        [Fact]
        public async Task GetProduct_WithNotExistingId_ReturnNotFound()
        {
            // Arrange
            var productId = 2;

            // Act
            var res = await _client.GetAsync($"/api/Product/{productId}");


            // Assert
            //res.EnsureSuccessStatusCode();
            //var product = await res.Content.ReadFromJsonAsync<Product>();

            Assert.Equal(HttpStatusCode.NotFound,res.StatusCode);
        }

        //https://localhost:7043/api/Product

        //[Fact]
        //public async Task CreateProduct_ReturnCreateAtActionAndProduct()
        //{
        //    // Arrange
        //    var product = new Product { Name = "New Product", Price = 20 };


        //    // Act
        //    var res = await _client.PostAsJsonAsync("/api/Product", product);

        //    // Assert
        //    res.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.Created, res.StatusCode);

        //    var newProduct = await res.Content.ReadFromJsonAsync<Product>();

        //    Assert.NotNull(newProduct);
        //    Assert.Equal(product.Name,newProduct.Name);

        //}


        [Fact]
        public async Task CreateProduct_ReturnsCreatedAtActionResult_WithProduct()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 100 };
            //_mockProductService.Setup(service => service.CreateProduct(product)).ReturnsAsync(1);

            // Act
            var response = await _client.PostAsJsonAsync("/api/Product", product);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(createdProduct);
            //Assert.Equal(1, createdProduct.Id);
            Assert.Equal("Test Product", createdProduct.Name);
            Assert.Equal(100, createdProduct.Price);
        }


        //https://localhost:7043/api/Product/1002
        [Fact]
        public async Task DeleteProduct_WithExitingId_ReturnNoContent()
        {
            // Insert produ ... ID
            // Delete ID

            // Arrange
            var newProduct = new Product { Name = "Test New Product to Delete...", Price = 30 };
            var resnewProduct = await  _client.PostAsJsonAsync("/api/Product", newProduct);
            var resPro = await resnewProduct.Content.ReadFromJsonAsync<Product>();


            // Act
            var resDelete = await _client.DeleteAsync($"/api/Product/{resPro.Id}");


            // Assert
            Assert.Equal(HttpStatusCode.NoContent, resDelete.StatusCode);
        }

        //https://localhost:7043/api/Product/1002
        [Fact]
        public async Task DeleteProduct_WithNotExitingId_ReturnNotFound()
        {
            // Insert produ ... ID
            // Delete ID

            // Arrange
            //var newProduct = new Product { Name = "Test New Product to Delete...", Price = 30 };
            //var resnewProduct = await _client.PostAsJsonAsync("/api/Product", newProduct);
            //var resPro = await resnewProduct.Content.ReadFromJsonAsync<Product>();

            var productId = 999;

            // Act
            var resDelete = await _client.DeleteAsync($"/api/Product/{productId}");


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, resDelete.StatusCode);
        }

        //TODO ... Delete..
       
        [Theory]
        [InlineData(5,5, HttpStatusCode.NoContent)]
        [InlineData(5, 7, HttpStatusCode.BadRequest)]
        [InlineData(999, 999, HttpStatusCode.NotFound)]
        public async Task UpdateProduct_ReturnExpectedStatusCode(int id, int productId,HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var updsteProduct = new Product { Id = productId, Name = "Update Product", Price = 100 };


            // Act
            var res = await _client.PutAsJsonAsync($"/api/Product/{id}", updsteProduct);

            // Assert
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

    }
}
 
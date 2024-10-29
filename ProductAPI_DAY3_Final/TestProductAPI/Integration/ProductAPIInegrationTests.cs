using Azure;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductAPI;
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

        [Fact]
        public async Task CreateProduct_ReturnCreateAtActionAndProduct()
        {
            // Arrange
            var product = new Product { Name = "New Product", Price = 20 };


            // Act
            var res = await _client.PostAsJsonAsync("/api/Product", product);

            // Assert
            res.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, res.StatusCode);

            var newProduct = await res.Content.ReadFromJsonAsync<Product>();

            Assert.NotNull(newProduct);
            Assert.Equal(product.Name,newProduct.Name);

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

        //https://localhost:7043/api/Product/5
        [Fact]
        public async Task UpdateProduct_WithExistingId_ReturnNoContent()
        {
            // Arrange
            var updateId = 5;
            var productUpdate = new Product { Id = updateId, Name = "Test Product Update", Price = 10 };


            // Act
            var res = await _client.PutAsJsonAsync($"/api/Product/{updateId}", productUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);

        }

        //https://localhost:7043/api/Product/5
        [Fact]
        public async Task UpdateProduct_WithMismatchId_ReturnBadRequest()
        {
            // Arrange
            var updateId = 5;
            var productUpdate = new Product { Id = 7, Name = "Test Product Update", Price = 10 };


            // Act
            var res = await _client.PutAsJsonAsync($"/api/Product/{updateId}", productUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);

        }

        //https://localhost:7043/api/Product/5
        [Fact]
        public async Task UpdateProduct_WithNotExistongId_ReturnNotfound()
        {
            // Arrange
            var updateId = 999;
            var productUpdate = new Product { Id = 999, Name = "Test Product Update", Price = 10 };


            // Act
            var res = await _client.PutAsJsonAsync($"/api/Product/{updateId}", productUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);

        }

    }
}

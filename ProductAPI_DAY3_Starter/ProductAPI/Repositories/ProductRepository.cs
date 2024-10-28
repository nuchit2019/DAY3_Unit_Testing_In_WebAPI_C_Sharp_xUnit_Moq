using Dapper;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Repositories
{

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductById(int id);
        Task<int> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
    }
    public class ProductRepository : IProductRepository
    {

        private readonly IDapperContext _dapperContext;
        private readonly IDapperWrapper _dapperWrapper;
        public ProductRepository(IDapperContext dapperContext, IDapperWrapper dapperWrapper)
        {
            _dapperContext = dapperContext;
            _dapperWrapper = dapperWrapper;
        }
     
        public async Task<int> CreateProduct(Product product)
        {
          using(var connection = _dapperContext.CreateConnection())
            {
                string sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT SCOPE_IDENTITY();";
                //return await connection.ExecuteScalarAsync<int>(sql, product);
                return await _dapperWrapper.ExecuteScalarAsync<int>(connection, sql, product);

            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "delete products where Id=@Id";
                //return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
                return await _dapperWrapper.ExecuteAsync(connection, sql, new { Id = id }) > 0;

            }
        }

        public async Task<Product> GetProductById(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "select * from Products where Id=@ID";

                //return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { ID = id });
                return await _dapperWrapper.QueryFirstOrDefaultAsync<Product>(connection, sql, new { ID = id });

            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                //return await connection.QueryAsync<Product>("select * from Products");

                string sql = "SELECT * FROM Products";
                //return await _dapperWrapper.QueryAsync<Product>(connection, sql);
                return await _dapperWrapper.QueryAsync<Product>(connection, sql);


            }

        }

        public async Task<bool> UpdateProduct(Product product)
        {
           using(var connection = _dapperContext.CreateConnection())
            {
                string sql = "update products set name = @name,price = @price where id=@id";

                //return await connection.ExecuteAsync(sql, product) > 0;
                return await _dapperWrapper.ExecuteAsync(connection, sql, product) > 0;

            }
        }
    }

}

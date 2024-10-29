using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace ProductAPI.Data
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }

   // [ExcludeFromCodeCoverage]
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("LocaldbConnection");

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        
    }
}

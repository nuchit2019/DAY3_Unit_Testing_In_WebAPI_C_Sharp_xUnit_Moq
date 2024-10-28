using Dapper;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace ProductAPI.Data
{
    public interface IDapperWrapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null);
        Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string sql, object param = null);
    }

    //[ExcludeFromCodeCoverage]
    public class DapperWrapper : IDapperWrapper
    {
        public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return await connection.QueryAsync<T>(sql, param);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null)
        {
            return await connection.ExecuteAsync(sql, param);
        }

        public async Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return await connection.ExecuteScalarAsync<T>(sql, param);
        }
    }
}

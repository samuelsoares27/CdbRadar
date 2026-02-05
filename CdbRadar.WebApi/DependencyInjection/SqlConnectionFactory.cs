using CdbRadar.Repository.Abstractions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CdbRadar.WebApi.DependencyInjection
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;
        public SqlConnectionFactory(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
        }
    }
}

using CdbRadar.Repository.Abstractions;
using CdbRadar.Repository.Interfaces;
using Dapper;

namespace CdbRadar.Repository.Repositories
{
    public class TipoAtivoRepository(IDbConnectionFactory factory) : ITipoAtivoRepository
    {
        private readonly IDbConnectionFactory _factory = factory;

        public async Task<int?> ObterIdPorNomeAsync(string nome)
        {
            var sql = "SELECT Id FROM TiposAtivo WHERE Nome = @Nome";

            using var con = _factory.CreateConnection();

            return await con.QueryFirstOrDefaultAsync<int?>(sql, new { Nome = nome });
        }
    }
}

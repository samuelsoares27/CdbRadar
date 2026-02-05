using CdbRadar.Domain.Model;
using CdbRadar.Repository.Abstractions;
using CdbRadar.Repository.Interfaces;
using Dapper;

namespace CdbRadar.Repository.Repositories
{
    public class OfertaRepository(IDbConnectionFactory factory) : IOfertaRepository
    {
        private readonly IDbConnectionFactory _factory = factory;

        public async Task InserirAsync(CdbOfertas oferta)
        {
            var sql = @"
                INSERT INTO CdbOfertas
                    (TipoId, Nome, Emissor, Vencimento,
                     TaxaDescricao, TaxaValor, Indexador,
                     MinimoAplicacao, Liquidez, Mercado, Plataforma)
                VALUES
                    (@TipoId, @Nome, @Emissor, @Vencimento,
                     @TaxaDescricao, @TaxaValor, @Indexador,
                     @MinimoAplicacao, @Liquidez, @Mercado, @Plataforma)";

            using var con = _factory.CreateConnection();
            await con.ExecuteAsync(sql, oferta);
        }

        public async Task<List<CdbOfertas>> ListarAsync()
        {
            var sql = "SELECT * FROM CdbOfertas";

            using var con = _factory.CreateConnection();

            var result = await con.QueryAsync<CdbOfertas>(sql);

            return result.ToList();
        }

        public async Task<CdbOfertas?> ObterPorIdAsync(int id)
        {
            var sql = "SELECT * FROM CdbOfertas WHERE Id = @Id";

            using var con = _factory.CreateConnection();

            return await con.QueryFirstOrDefaultAsync<CdbOfertas>(sql, new { Id = id });
        }

        public async Task<bool> AtualizarAsync(CdbOfertas oferta)
        {
            var sql = @"
                UPDATE CdbOfertas
                SET
                    TipoId          = @TipoId,
                    Nome            = @Nome,
                    Emissor         = @Emissor,
                    Vencimento      = @Vencimento,
                    TaxaDescricao   = @TaxaDescricao,
                    TaxaValor       = @TaxaValor,
                    Indexador       = @Indexador,
                    MinimoAplicacao = @MinimoAplicacao,
                    Liquidez        = @Liquidez,
                    Mercado         = @Mercado,
                    Plataforma      = @Plataforma,
                    DataColeta      = @DataColeta
                WHERE Id = @Id;
            ";

            using var con = _factory.CreateConnection();

            var linhasAfetadas = await con.ExecuteAsync(sql, oferta);

            return linhasAfetadas > 0;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var sql = @"DELETE FROM CdbOfertas WHERE Id = @Id";

            using var con = _factory.CreateConnection();

            var linhasAfetadas = await con.ExecuteAsync(sql, new { Id = id });

            return linhasAfetadas > 0;
        }
    }
}

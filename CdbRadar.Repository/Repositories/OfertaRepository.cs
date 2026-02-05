using CdbRadar.Domain.Model;
using CdbRadar.Repository.Abstractions;
using CdbRadar.Repository.Interfaces;
using Dapper;
using System.Text;

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

        public async Task<IEnumerable<CdbOfertas>> ListarAsync(CdbOfertasFiltro filtro)
        {
            var sql = new StringBuilder("SELECT * FROM CdbOfertas WHERE 1=1 ");
            var p = new DynamicParameters();

            if (filtro.Id.HasValue)
            {
                sql.Append(" AND Id = @Id");
                p.Add("Id", filtro.Id);
            }

            if (filtro.TipoId.HasValue)
            {
                sql.Append(" AND TipoId = @TipoId");
                p.Add("TipoId", filtro.TipoId);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                sql.Append(" AND Nome LIKE @Nome");
                p.Add("Nome", $"%{filtro.Nome}%");
            }

            if (!string.IsNullOrWhiteSpace(filtro.Emissor))
            {
                sql.Append(" AND Emissor LIKE @Emissor");
                p.Add("Emissor", $"%{filtro.Emissor}%");
            }

            if (filtro.VencimentoDe.HasValue)
            {
                sql.Append(" AND Vencimento >= @VencimentoDe");
                p.Add("VencimentoDe", filtro.VencimentoDe);
            }

            if (filtro.VencimentoAte.HasValue)
            {
                sql.Append(" AND Vencimento <= @VencimentoAte");
                p.Add("VencimentoAte", filtro.VencimentoAte);
            }

            if (!string.IsNullOrWhiteSpace(filtro.TaxaDescricao))
            {
                sql.Append(" AND TaxaDescricao LIKE @TaxaDescricao");
                p.Add("TaxaDescricao", $"%{filtro.TaxaDescricao}%");
            }

            if (filtro.TaxaValorMin.HasValue)
            {
                sql.Append(" AND TaxaValor >= @TaxaValorMin");
                p.Add("TaxaValorMin", filtro.TaxaValorMin);
            }

            if (filtro.TaxaValorMax.HasValue)
            {
                sql.Append(" AND TaxaValor <= @TaxaValorMax");
                p.Add("TaxaValorMax", filtro.TaxaValorMax);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Indexador))
            {
                sql.Append(" AND Indexador LIKE @Indexador");
                p.Add("Indexador", $"%{filtro.Indexador}%");
            }

            if (filtro.MinimoAplicacaoMin.HasValue)
            {
                sql.Append(" AND MinimoAplicacao >= @MinimoAplicacaoMin");
                p.Add("MinimoAplicacaoMin", filtro.MinimoAplicacaoMin);
            }

            if (filtro.MinimoAplicacaoMax.HasValue)
            {
                sql.Append(" AND MinimoAplicacao <= @MinimoAplicacaoMax");
                p.Add("MinimoAplicacaoMax", filtro.MinimoAplicacaoMax);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Liquidez))
            {
                sql.Append(" AND Liquidez LIKE @Liquidez");
                p.Add("Liquidez", $"%{filtro.Liquidez}%");
            }

            if (!string.IsNullOrWhiteSpace(filtro.Mercado))
            {
                sql.Append(" AND Mercado LIKE @Mercado");
                p.Add("Mercado", $"%{filtro.Mercado}%");
            }

            if (!string.IsNullOrWhiteSpace(filtro.Plataforma))
            {
                sql.Append(" AND Plataforma LIKE @Plataforma");
                p.Add("Plataforma", $"%{filtro.Plataforma}%");
            }

            if (filtro.DataColetaDe.HasValue)
            {
                sql.Append(" AND DataColeta >= @DataColetaDe");
                p.Add("DataColetaDe", filtro.DataColetaDe);
            }

            if (filtro.DataColetaAte.HasValue)
            {
                sql.Append(" AND DataColeta <= @DataColetaAte");
                p.Add("DataColetaAte", filtro.DataColetaAte);
            }

            // Ordenação segura (evita SQL Injection)
            var colunasPermitidas = new[]
            {
                "Id","TipoId","Nome","Emissor","Vencimento",
                "TaxaValor","Indexador","MinimoAplicacao",
                "Liquidez","Mercado","Plataforma","DataColeta"
            };

            var ordenarPor = colunasPermitidas.Contains(filtro.OrdenarPor ?? "")
                ? filtro.OrdenarPor
                : "Id";

            var direcao = (filtro.Direcao?.ToUpper() == "DESC") ? "DESC" : "ASC";

            sql.Append($" ORDER BY {ordenarPor} {direcao}");

            // Paginação
            var offset = (filtro.Pagina - 1) * filtro.TamanhoPagina;

            sql.Append(" OFFSET @Offset ROWS FETCH NEXT @TamanhoPagina ROWS ONLY");
            p.Add("Offset", offset);
            p.Add("TamanhoPagina", filtro.TamanhoPagina);

            using var con = _factory.CreateConnection();
            return await con.QueryAsync<CdbOfertas>(sql.ToString(), p);
        }
    }
}

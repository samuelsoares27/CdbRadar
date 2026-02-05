namespace CdbRadar.Domain.Model
{
    public class CdbOfertasFiltro
    {
        public int? Id { get; set; }
        public int? TipoId { get; set; }
        public string? Nome { get; set; }
        public string? Emissor { get; set; }
        public DateTime? VencimentoDe { get; set; }
        public DateTime? VencimentoAte { get; set; }
        public string? TaxaDescricao { get; set; }
        public decimal? TaxaValorMin { get; set; }
        public decimal? TaxaValorMax { get; set; }
        public string? Indexador { get; set; }
        public decimal? MinimoAplicacaoMin { get; set; }
        public decimal? MinimoAplicacaoMax { get; set; }
        public string? Liquidez { get; set; }
        public string? Mercado { get; set; }
        public string? Plataforma { get; set; }
        public DateTime? DataColetaDe { get; set; }
        public DateTime? DataColetaAte { get; set; }

        // paginação
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 20;

        // ordenação
        public string? OrdenarPor { get; set; } = "Id";
        public string? Direcao { get; set; } = "ASC";
    }
}

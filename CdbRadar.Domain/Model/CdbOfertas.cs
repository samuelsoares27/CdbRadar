namespace CdbRadar.Domain.Model
{
    public class CdbOfertas
    {
        public int Id { get; set; }
        public int TipoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Emissor { get; set; } = string.Empty;
        public DateTime Vencimento { get; set; }
        public string TaxaDescricao { get; set; } = string.Empty;
        public decimal TaxaValor { get; set; }
        public string Indexador { get; set; } = string.Empty;
        public decimal MinimoAplicacao { get; set; }
        public string Liquidez { get; set; } = string.Empty;
        public string Mercado { get; set; } = string.Empty;
        public string Plataforma { get; set; } = "BTG";
        public DateTime DataColeta { get; set; }
    }
}

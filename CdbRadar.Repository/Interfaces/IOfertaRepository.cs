using CdbRadar.Domain.Model;

namespace CdbRadar.Repository.Interfaces
{
    public interface IOfertaRepository
    {
        Task InserirAsync(CdbOfertas oferta);
        Task<List<CdbOfertas>> ListarAsync();
        Task<CdbOfertas?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(CdbOfertas oferta);
        Task<bool> DeletarAsync(int id);
        Task<IEnumerable<CdbOfertas>> ListarAsync(CdbOfertasFiltro filtro);
    }
}

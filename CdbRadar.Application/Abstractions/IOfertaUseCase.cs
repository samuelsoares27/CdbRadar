using CdbRadar.Application.DTOs;
using CdbRadar.Domain.Model;

namespace CdbRadar.Application.Abstractions
{
    public interface IOfertaUseCase
    {
        public Task SalvarAsync(CdbOfertasDto oferta);
        public Task<List<CdbOfertasDto>> ObterTodosAsync();
        public Task<CdbOfertas?> ObterPorIdAsync(int id);
        public Task<bool> AtualizarAsync(int id, CdbOfertasDto oferta);
        public Task<bool> DeletarAsync(int id);
        Task<List<CdbOfertas>> ListarAsync(CdbOfertasFiltro filtro);
    }
}

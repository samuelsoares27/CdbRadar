using CdbRadar.Application.Abstractions;
using CdbRadar.Application.DTOs;
using CdbRadar.Domain.Model;
using CdbRadar.Repository.Interfaces;
using Mapster;
using MapsterMapper;
using static Dapper.SqlMapper;

namespace CdbRadar.Application.UseCases
{
    public class OfertaUseCase(IOfertaRepository repositorio, IMapper mapper) : IOfertaUseCase
    {
        private readonly IOfertaRepository _repositorio = repositorio;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> AtualizarAsync(int id, CdbOfertasDto oferta)
        {
            var cdbOfertas = oferta.Adapt<CdbOfertas>();
            cdbOfertas.Id = id;

            var ok = await _repositorio.AtualizarAsync(cdbOfertas);
            if (ok)
                return true;
            return false;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var ok = await _repositorio.DeletarAsync(id);
            if (ok)
                return true;
            return false;
        }

        public async Task<CdbOfertas?> ObterPorIdAsync(int id)
        {
            var cdbOfertasDto = await _repositorio.ObterPorIdAsync(id);
            return cdbOfertasDto;
        }

        public async Task<List<CdbOfertasDto>> ObterTodosAsync()
        {
            var lista = await _repositorio.ListarAsync();
            return lista.Adapt<List<CdbOfertasDto>>();
        }


        public async Task SalvarAsync(CdbOfertasDto dto)
        {
            var entity = _mapper.Map<CdbOfertas>(dto);
            await _repositorio.InserirAsync(entity);
        }

        public async Task<List<CdbOfertas>> ListarAsync(CdbOfertasFiltro filtro)
        {
            var lista = await _repositorio.ListarAsync(filtro);
            return lista.Adapt<List<CdbOfertas>>();
        }

    }
}

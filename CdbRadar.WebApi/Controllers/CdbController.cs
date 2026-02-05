using CdbRadar.Application.Abstractions;
using CdbRadar.Application.DTOs;
using CdbRadar.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace CdbRadar.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CdbController(IOfertaUseCase useCase) : ControllerBase
    {
        private readonly IOfertaUseCase _useCase = useCase;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CdbOfertasDto dto)
        {
            await _useCase.SalvarAsync(dto);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<CdbOfertasDto>>> Get()
        {
            var cdbOfertasDto = await _useCase.ObterTodosAsync();

            if(cdbOfertasDto.Count == 0)
                return NoContent();

            return Ok(cdbOfertasDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CdbOfertasDto>> Get(int id)
        {
            var cdbOfertasDto = await _useCase.ObterPorIdAsync(id);
            if (cdbOfertasDto is null)
                return NotFound();

            return Ok(cdbOfertasDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CdbOfertasDto dto)
        {
            var ok = await _useCase.AtualizarAsync(id, dto);

            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _useCase.DeletarAsync(id);

            return ok ? NoContent() : NotFound();
        }

        [HttpGet("ListarAsync")]
        public async Task<ActionResult<IEnumerable<CdbOfertasDto>>> ListarAsync(
            [FromQuery] CdbOfertasFiltro filtro)
        {
            var cdbOfertasDto = await _useCase.ListarAsync(filtro);

            if (cdbOfertasDto == null || !cdbOfertasDto.Any())
                return NoContent();

            return Ok(cdbOfertasDto);
        }

    }
}

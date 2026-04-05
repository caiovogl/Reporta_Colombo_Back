using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Reporta_Colombo_Back.DTOs;
using Reporta_Colombo_Back.Models;
using Reporta_Colombo_Back.Services;
using Reporta_Colombo_Back.Services.Interfaces;

namespace Reporta_Colombo_Back.Controllers
{
    [ApiController]
    public class DenunciaController : ControllerBase
    {
        private readonly IDenunciaService _denunciaService;
        
        public DenunciaController(IDenunciaService DenunciaService)
        {
            _denunciaService = DenunciaService;
        }

        [HttpPost("Adicionar-Denuncia")]
        public async Task<IActionResult> Post([FromForm] DenunciaDto denunciaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Resultado = await _denunciaService.AddDenunciaAsync(denunciaDto);

                return Ok(new { message = "Denúncia ambiental registrada com sucesso!", Resultado.Url_Imagem });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("Buscar-Denuncias-Filtrado")]
        public async Task<IActionResult> Filtro([FromBody] DenunciaFilterInput filtro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Resultado = await _denunciaService.GetDenunciasFiltered(filtro);

                return Ok(Resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_ERP.DTO;
using Service_ERP.IService;

namespace ERP.Controllers
{
    /// <summary>
    /// Lignes de commande (table de jointure Commande-Article)
    /// Clé composite : IdCom + IdArt
    /// </summary>
    [ApiController]
    [Route("ERP/[controller]")]
    [Authorize]
    public class LCommandeController : ControllerBase
    {
        private readonly ILCommandeService _service;

        public LCommandeController(ILCommandeService service)
        {
            _service = service;
        }

        // GET /LCommande
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET /LCommande/{idCom}/{idArt}
        [HttpGet("{idCom:int}/{idArt:int}")]
        public async Task<IActionResult> GetById(int idCom, int idArt)
        {
            var result = await _service.GetByIdAsync(idCom, idArt);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST /LCommande
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LCommandeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idCom = created.IdCom, idArt = created.IdArt }, created);
        }

        // PUT /LCommande/{idCom}/{idArt}
        [HttpPut("{idCom:int}/{idArt:int}")]
        public async Task<IActionResult> Update(int idCom, int idArt, [FromBody] LCommandeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (idCom != dto.IdCom || idArt != dto.IdArt) return BadRequest("Les identifiants ne correspondent pas.");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        // DELETE /LCommande/{idCom}/{idArt}
        [HttpDelete("{idCom:int}/{idArt:int}")]
        public async Task<IActionResult> Delete(int idCom, int idArt)
        {
            await _service.DeleteAsync(idCom, idArt);
            return NoContent();
        }
    }
}

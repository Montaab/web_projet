using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_ERP.DTO;
using Service_ERP.IService;

namespace ERP.Controllers
{
    /// <summary>
    /// Lignes de panier (table de jointure Panier-Article)
    /// Clé composite : IdPan + IdArt
    /// </summary>
    [ApiController]
    [Route("ERP/[controller]")]
    [Authorize(Roles = "Administrateur")]

    public class LPanierController : ControllerBase
    {
        private readonly ILPanierService _service;

        public LPanierController(ILPanierService service)
        {
            _service = service;
        }

        // GET /LPanier
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET /LPanier/{idPan}/{idArt}
        [HttpGet("{idPan:int}/{idArt:int}")]
        public async Task<IActionResult> GetById(int idPan, int idArt)
        {
            var result = await _service.GetByIdAsync(idPan, idArt);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST /LPanier
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LPanierDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idPan = created.IdPan, idArt = created.IdArt }, created);
        }

        // PUT /LPanier/{idPan}/{idArt}
        [HttpPut("{idPan:int}/{idArt:int}")]
        public async Task<IActionResult> Update(int idPan, int idArt, [FromBody] LPanierDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (idPan != dto.IdPan || idArt != dto.IdArt) return BadRequest("Les identifiants ne correspondent pas.");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        // DELETE /LPanier/{idPan}/{idArt}
        [HttpDelete("{idPan:int}/{idArt:int}")]
        public async Task<IActionResult> Delete(int idPan, int idArt)
        {
            await _service.DeleteAsync(idPan, idArt);
            return NoContent();
        }
    }
}

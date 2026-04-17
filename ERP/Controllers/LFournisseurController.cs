using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_ERP.DTO;
using Service_ERP.IService;

namespace ERP.Controllers
{
    /// <summary>
    /// Lignes fournisseur (table de jointure Fournisseur-Article)
    /// Clé composite : IdFour + IdArt
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LFournisseurController : ControllerBase
    {
        private readonly ILFournisseurService _service;

        public LFournisseurController(ILFournisseurService service)
        {
            _service = service;
        }

        // GET /LFournisseur
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET /LFournisseur/{idFour}/{idArt}
        [HttpGet("{idFour:int}/{idArt:int}")]
        public async Task<IActionResult> GetById(int idFour, int idArt)
        {
            var result = await _service.GetByIdAsync(idFour, idArt);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST /LFournisseur
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LFournisseurDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idFour = created.IdFour, idArt = created.IdArt }, created);
        }

        // PUT /LFournisseur/{idFour}/{idArt}
        [HttpPut("{idFour:int}/{idArt:int}")]
        public async Task<IActionResult> Update(int idFour, int idArt, [FromBody] LFournisseurDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (idFour != dto.IdFour || idArt != dto.IdArt) return BadRequest("Les identifiants ne correspondent pas.");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        // DELETE /LFournisseur/{idFour}/{idArt}
        [HttpDelete("{idFour:int}/{idArt:int}")]
        public async Task<IActionResult> Delete(int idFour, int idArt)
        {
            await _service.DeleteAsync(idFour, idArt);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_ERP.DTO;
using Service_ERP.IService;

namespace ERP.Controllers
{
    [ApiController]
    [Route("ERP/[controller]")]
    [Authorize]
    public class SouscategorieController : ControllerBase
    {
        private readonly ISouscategorieService _service;

        public SouscategorieController(ISouscategorieService service)
        {
            _service = service;
        }

        // GET /Souscategorie
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET /Souscategorie/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST /Souscategorie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SouscategorieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdScat }, created);
        }

        // PUT /Souscategorie/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SouscategorieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.IdScat) return BadRequest("L'identifiant ne correspond pas.");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        // DELETE /Souscategorie/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

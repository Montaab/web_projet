using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_ERP.DTO;
using Service_ERP.IService;

namespace ERP.Controllers
{
    [ApiController]
    [Route("ERP/[controller]")]
    [Authorize]
    public class PanierController : ControllerBase
    {
        private readonly IPanierService _service;

        public PanierController(IPanierService service)
        {
            _service = service;
        }

        // GET /Panier
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET /Panier/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST /Panier
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PanierDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdPan }, created);
        }

        // PUT /Panier/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PanierDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.IdPan) return BadRequest("L'identifiant ne correspond pas.");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        // DELETE /Panier/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_ERP.DTO;
using Service_ERP.IService;

namespace ERP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CommandeController : ControllerBase
    {
        private readonly ICommandeService _service;

        public CommandeController(ICommandeService service)
        {
            _service = service;
        }

        // GET /Commande
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET /Commande/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST /Commande
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommandeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdCom }, created);
        }

        // PUT /Commande/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommandeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.IdCom) return BadRequest("L'identifiant ne correspond pas.");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        // DELETE /Commande/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

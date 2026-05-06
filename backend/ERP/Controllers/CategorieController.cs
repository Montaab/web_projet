using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_ERP.DTO;
using Service_ERP.IService;

namespace ERP.Controllers
{
    [ApiController]
    [Route("ERP/[controller]")]
    [Authorize(Roles = "Administrateur,Gestionnaire")]

    public class CategorieController : ControllerBase
    {
        private readonly ICategorieService _service;

        public CategorieController(ICategorieService service)
        {
            _service = service;
        }

        // GET /Categorie
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET /Categorie/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST /Categorie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategorieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdCat }, created);
        }

        // PUT /Categorie/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategorieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.IdCat) return BadRequest("L'identifiant ne correspond pas.");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        // DELETE /Categorie/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

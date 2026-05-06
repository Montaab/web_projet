using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.IService;

namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("Role")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        private readonly Serilog.ILogger _logger;

        public RoleController(IRoleService service, Serilog.ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        // =========================
        // GET ALL ROLES
        // =========================
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RolesDto>>> GetAll()
        {
            try
            {
                var roles = await _service.GetAllAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur GetAll Roles: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // GET ROLE BY ID
        // =========================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RolesDto>> GetById(int id)
        {
            try
            {
                var role = await _service.GetByIdAsync(id);

                if (role == null)
                    return NotFound(new { Message = "Rôle non trouvé" });

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur GetById Role {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // CREATE ROLE
        // =========================
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RolesDto>> Create([FromBody] RolesDto roleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdRole = await _service.AddAsync(roleDto);
                return CreatedAtAction(nameof(GetById), new { id = createdRole.Idrole }, createdRole);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Create Role: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // UPDATE ROLE
        // =========================
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] RolesDto roleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != roleDto.Idrole)
                    return BadRequest(new { Message = "ID mismatch" });

                var existingRole = await _service.GetByIdAsync(id);
                if (existingRole == null)
                    return NotFound(new { Message = "Rôle non trouvé" });

                await _service.UpdateAsync(roleDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Update Role {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // DELETE ROLE
        // =========================
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingRole = await _service.GetByIdAsync(id);
                if (existingRole == null)
                    return NotFound(new { Message = "Rôle non trouvé" });

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Delete Role {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }
    }
}
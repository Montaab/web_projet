using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.IService;
using Service.Models;

namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly IUtilisateurService _service;
        private readonly Serilog.ILogger _logger;

        public UtilisateurController(IUtilisateurService service, Serilog.ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("IsLogin")]
        public async Task<ActionResult<ResponseLogin>> Login([FromBody] Login login)
        {
            try
            {
                var result = await _service.Login(login);

                if (result == null)
                    return Unauthorized(new { Message = "Username ou mot de passe incorrect" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Login: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // GET ALL
        // =========================
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilisateurDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilisateurDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // =========================
        // ADD
        // =========================
        [HttpPost]
        public async Task<ActionResult<UtilisateurDto>> Add([FromBody] UtilisateurDto dto)
        {
            try
            {
                var result = await _service.AddAsync(dto);

                if (result == null)
                    return BadRequest(new { Message = "Email déjà existant" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur AddUser: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // UPDATE
        // =========================
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UtilisateurDto dto)
        {
            if (id != dto.Iduser)
                return BadRequest();

            try
            {
                await _service.UpdateAsync(dto);
                return Ok(new { Message = "Mise à jour réussie" });
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur UpdateUser: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // DELETE
        // =========================
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { Message = "Suppression réussie" });
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur DeleteUser: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }
    }
}
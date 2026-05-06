using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.IService;

namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("User/[controller]")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;
        private readonly Serilog.ILogger _logger;

        public ProfileController(IProfileService service, Serilog.ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        // =========================
        // GET ALL PROFILES
        // =========================
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> GetAll()
        {
            try
            {
                var profiles = await _service.GetAllAsync();
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur GetAll Profiles: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // GET PROFILE BY ID
        // =========================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProfileDto>> GetById(int id)
        {
            try
            {
                var profile = await _service.GetByIdAsync(id);

                if (profile == null)
                    return NotFound(new { Message = "Profil non trouvé" });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur GetById Profile {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // CREATE PROFILE
        // =========================
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProfileDto>> Create([FromBody] ProfileDto profileDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdProfile = await _service.AddAsync(profileDto);
                return CreatedAtAction(nameof(GetById), new { id = createdProfile.Idprofil }, createdProfile);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Create Profile: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // UPDATE PROFILE
        // =========================
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] ProfileDto profileDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != profileDto.Idprofil)
                    return BadRequest(new { Message = "ID mismatch" });

                var existingProfile = await _service.GetByIdAsync(id);
                if (existingProfile == null)
                    return NotFound(new { Message = "Profil non trouvé" });

                await _service.UpdateAsync(profileDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Update Profile {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // DELETE PROFILE
        // =========================
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingProfile = await _service.GetByIdAsync(id);
                if (existingProfile == null)
                    return NotFound(new { Message = "Profil non trouvé" });

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Delete Profile {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }
    }
}
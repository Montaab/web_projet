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
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;
        private readonly Serilog.ILogger _logger;

        public MenuController(IMenuService service, Serilog.ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        // =========================
        // GET ALL MENUS
        // =========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetAll()
        {
            try
            {
                var menus = await _service.GetAllAsync();
                return Ok(menus);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur GetAll Menus: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // GET MENUS BY ROLE ID
        // =========================
        [HttpGet("ByRole/{roleId}")]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetByRole(int roleId)
        {
            try
            {
                var menus = await _service.GetByRoleIdAsync(roleId);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur GetByRole Menus (roleId={roleId}): {ex}");
                return StatusCode(500, new
                {
                    Message = ex.Message,
                    Detail = ex.InnerException?.Message
                });
            }
        }

        // =========================
        // GET MENU BY ID
        // =========================
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrateur")]
        public async Task<ActionResult<MenuDto>> GetById(int id)
        {
            try
            {
                var menu = await _service.GetByIdAsync(id);

                if (menu == null)
                    return NotFound(new { Message = "Menu non trouvé" });

                return Ok(menu);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur GetById Menu {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // CREATE MENU
        // =========================
        [HttpPost]
        [Authorize(Roles = "Administrateur")]
        public async Task<ActionResult<MenuDto>> Create([FromBody] MenuDto menuDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdMenu = await _service.AddAsync(menuDto);
                return CreatedAtAction(nameof(GetById), new { id = createdMenu.Idmenu }, createdMenu);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Create Menu: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // UPDATE MENU
        // =========================
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuDto menuDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != menuDto.Idmenu)
                    return BadRequest(new { Message = "ID mismatch" });

                var existingMenu = await _service.GetByIdAsync(id);
                if (existingMenu == null)
                    return NotFound(new { Message = "Menu non trouvé" });

                await _service.UpdateAsync(menuDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Update Menu {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }

        // =========================
        // DELETE MENU
        // =========================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingMenu = await _service.GetByIdAsync(id);
                if (existingMenu == null)
                    return NotFound(new { Message = "Menu non trouvé" });

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur Delete Menu {id}: {ex}");
                return StatusCode(500, new { Message = "Erreur serveur" });
            }
        }
    }
}
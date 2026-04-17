using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.IService;
using Service.Models;

namespace Auth.API.Controllers
{
    [Produces("application/json")]
    [Route("Auth")]
    [EnableCors("CORSPolicy")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUtilisateurService _userService;

        public AuthController(IAuthService authService, IUtilisateurService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        /// <summary>
        /// Login endpoint
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var result = await _userService.Islogin(login);
            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost("GenToken")]
        public IActionResult Generate([FromBody] UtilisateurDto user)
        {
            var token = _authService.GenerateAccessToken(user);
            return Ok(new { token });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        [HttpPost("ValidateToken")]
        public IActionResult Validate([FromBody] string token)
        {
            var principal = _authService.ValidateToken(token);

            if (principal == null)
                return Unauthorized();

            var claims = principal.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new { valid = true, claims });
        }
    }
}


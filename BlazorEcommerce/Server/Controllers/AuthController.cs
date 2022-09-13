using BlazorEcommerce.Server.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister register)
        {
            var response = await _authService.Register(new User() { Email = register.Email }, register.Password);
            if (!response.Success)
            {
                BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var response = await _authService.Login(userLogin.Email, userLogin.Password);

            return Ok(response);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _authService.ChangePassword(int.Parse(userId), newPassword);
            return Ok(response);
        }
    }
}
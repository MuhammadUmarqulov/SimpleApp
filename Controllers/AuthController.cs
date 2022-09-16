using Microsoft.AspNetCore.Mvc;
using SimpleApp.Auth;
using SimpleApp.Models.DTOs;

namespace SimpleApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> LoginAsync(UserForLoginDto dto) =>
            Ok(new
            {
               Token = await authService.LoginAsync(dto)
            });

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Data.IRepositories;
using SimpleApp.Models.DTOs;
using SimpleApp.Models.Entities;
using System.Security.Claims;

namespace SimpleApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet("{Id}")]
        [Authorize]
        public async ValueTask<ActionResult<User>> GetAsync([FromRoute(Name = "Id")] long id) =>
            Ok(await userRepository.GetAsync(u => u.Id == id));

        [HttpDelete("{Id}")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteAsync([FromRoute(Name = "Id")] long id) =>
            Ok ( await userRepository.DeleteAsync(u => u.Id == id));

        [HttpGet]
        public async ValueTask<IActionResult> GetAllAsync() =>
            Ok(await userRepository.GetAll().ToListAsync());

        [HttpPost]
        public async ValueTask<IActionResult> CreateAsyc(UserForCreationDto dto) =>
            Ok(await userRepository.CreateAsync(dto));

        [HttpPut("{Id}")]
        public async ValueTask<IActionResult> UpdateAsync([FromRoute(Name = "Id")] long id, UserForCreationDto dto) =>
            Ok(await userRepository.UpdateAsync(id, dto));

        [HttpGet("Info")]
        [Authorize]
        public async ValueTask<ActionResult<User>> GetInfoAsync()
        {
            User user = await GetCurrentUserInfoAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        private async Task<User> GetCurrentUserInfoAsync()
        {
            var infoClaims = HttpContext.User.Identity as ClaimsIdentity;

            if (infoClaims != null)
            {
                var claims = infoClaims.Claims;

                var userid = long.Parse(claims.FirstOrDefault(p => p.Type == "Id")?.Value);

                var user = await userRepository.GetAsync(user => user.Id == userid);

                return user;
            }

            return null;
        }
    }
}
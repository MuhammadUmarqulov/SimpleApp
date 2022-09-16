using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Data.IRepositories;
using SimpleApp.Models.DTOs;

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
        public async ValueTask<IActionResult> GetAsync([FromRoute(Name = "Id")] long id) =>
            Ok(await userRepository.GetAsync(u => u.Id == id));

        [HttpDelete("{Id}")]
        public async ValueTask<IActionResult> DeleteAsync([FromRoute(Name = "Id")] long id) =>
            Ok ( await userRepository.DeleteAsync(u => u.Id == id));

        [HttpGet]
        public  async ValueTask<IActionResult> GetAllAsync() =>
            Ok(await userRepository.GetAll().ToListAsync());

        [HttpPost]
        public async ValueTask<IActionResult> CreateAsyc(UserForCreationDto dto) =>
            Ok(await userRepository.CreateAsync(dto));

        [HttpPut("{Id}")]
        public async ValueTask<IActionResult> UpdateAsync([FromRoute(Name = "Id")] long id, UserForCreationDto dto) =>
            Ok(await userRepository.UpdateAsync(id, dto));

    }
}
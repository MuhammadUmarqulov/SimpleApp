using SimpleApp.Models.DTOs;

namespace SimpleApp.Auth
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(UserForLoginDto dto);
    }
}

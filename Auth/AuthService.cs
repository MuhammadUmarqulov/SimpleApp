using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleApp.Data.DbContexts;
using SimpleApp.Exceptions;
using SimpleApp.Models.DTOs;
using SimpleApp.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleApp.Auth;

public class AuthService : IAuthService
{
    private readonly DbSet<User> dbSet;
    private readonly IConfiguration _configuration;

    public AuthService(SimpleDbContexts dbContext, IConfiguration configuration)
    {
        dbSet = dbContext.Users;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(UserForLoginDto dto)
    {
        var existUser = await dbSet.FirstOrDefaultAsync(
            user => user.Login == dto.Login 
            && user.Password == dto.Password);

        if (existUser is null)
            throw new MyException(404, "User not found.");

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

        var claims = new Claim[]
        {
            new Claim("Id", existUser.Id.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.Now.AddHours(12),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        string mainToken = new JwtSecurityTokenHandler().WriteToken(token);

        return mainToken;
    } 
}

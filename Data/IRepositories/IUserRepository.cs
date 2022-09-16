using SimpleApp.Models.DTOs;
using SimpleApp.Models.Entities;
using System.Linq.Expressions;

namespace SimpleApp.Data.IRepositories;

public interface IUserRepository
{
    Task<User> CreateAsync(UserForCreationDto user);
    Task<User> UpdateAsync(long id, UserForCreationDto user);
    Task<bool> DeleteAsync(Expression<Func<User, bool>> expression);

    Task<User> GetAsync(Expression<Func<User, bool>> expression);
    IQueryable<User> GetAll(Expression<Func<User, bool>> expression = null);
}

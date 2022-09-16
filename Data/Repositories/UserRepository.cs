using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SimpleApp.Data.DbContexts;
using SimpleApp.Data.IRepositories;
using SimpleApp.Exceptions;
using SimpleApp.Models.DTOs;
using SimpleApp.Models.Entities;
using System.Linq.Expressions;

namespace SimpleApp.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SimpleDbContexts dbContext;
    private readonly DbSet<User> dbSet;

    public UserRepository(SimpleDbContexts dbContext)
    {
        this.dbContext = dbContext;
        this.dbSet = dbContext.Users;
    }

    public async Task<User> CreateAsync(UserForCreationDto user)
    {
        User existUser = await dbSet.FirstOrDefaultAsync(u => u.Login == user.Login);

        if (existUser is not null)
            throw new MyException(400, "This login already exist!");

        var mappedUser = user.Adapt<User>();
        
        var newUser = await dbSet.AddAsync(mappedUser);
        await dbContext.SaveChangesAsync();

        return newUser.Entity;

    }

    public async Task<bool> DeleteAsync(Expression<Func<User, bool>> expression)
    {
        var existUser = await GetAsync(expression);

        if (existUser is null)
            return false;

        dbSet.Remove(existUser);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public IQueryable<User> GetAll(Expression<Func<User, bool>> expression = null) =>
        expression is null ? dbSet : dbSet.Where(expression); 
    

    public async Task<User> GetAsync(Expression<Func<User, bool>> expression)
    {
        var existUser = await dbSet.FirstOrDefaultAsync(expression);

        if (existUser is null)
            throw new MyException(404, "User not found.");

        return existUser;
    }

    public async Task<User> UpdateAsync(long id, UserForCreationDto user)
    {
        var existUser = await dbSet.FirstOrDefaultAsync(u => u.Id == id);

        if (existUser is null)
            throw new MyException(404, "User not found.");

        User checkedUser = await dbSet.FirstOrDefaultAsync(u => u.Login == user.Login);

        if (checkedUser is not null)
            throw new MyException(400, "This login already exist!");

        existUser = user.Adapt(existUser);
        existUser.UpdatedAt = DateTime.UtcNow;

        var newUser = dbSet.Update(existUser);
        await dbContext.SaveChangesAsync();

        return newUser.Entity;
    }
}

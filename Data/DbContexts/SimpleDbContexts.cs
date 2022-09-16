using Microsoft.EntityFrameworkCore;
using SimpleApp.Models.Entities;

namespace SimpleApp.Data.DbContexts;

public class SimpleDbContexts : DbContext
{
    public virtual DbSet<User> Users { get; set; }

    public SimpleDbContexts(DbContextOptions<SimpleDbContexts> optionsBuilder)
        : base(optionsBuilder)
    {

    }
}

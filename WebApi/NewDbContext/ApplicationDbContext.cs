using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.NewDbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
        
    }
    public DbSet<User> User { get; set; }
    
}
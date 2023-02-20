using Kirel.Repositories.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Models;

public class User : IdentityUser<int>,ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public int Id { get; set; }
    public new string? UserNames { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public new string? Email { get; set; }
    public DateTime Created { get; set; }
    public string? Password { get; set; }
    
}
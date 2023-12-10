using Kirel.Repositories.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace Library.Models;

public class User : IdentityUser<int>, ICreatedAtTrackedEntity, IKeyEntity<int>, IUser<string>
{
    private string _id;
    public int Id { get; set; }

    string IUser<string>.Id => _id;

    public override string? UserName { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public new string? Email { get; set; }
    public DateTime Created { get; set; }
    public string? Password { get; set; }
}
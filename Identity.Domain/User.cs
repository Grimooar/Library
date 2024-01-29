using Kirel.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain;

public class User : IdentityUser<int>,ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public new string? Email { get; set; }
    public DateTime Created { get; set; }
    public string? Password { get; set; }
    
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }
    

}

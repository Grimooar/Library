using Microsoft.AspNetCore.Identity;

namespace Library.Models;

/// <summary>
/// The default implementation of <see cref="Microsoft.AspNet.Identity.EntityFramework.IdentityRole" /> which uses a string as the primary key.
/// </summary>
public class IdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim> : IdentityRole<TKey> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : IdentityRoleClaim<TKey>
    where TUserClaim : Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<TKey>
{
    /// <summary>
    /// List of user roles 
    /// </summary>
    public virtual ICollection<TUserRole> UserRoles { get; } = null!;
    /// <summary>
    /// Claims of the role
    /// </summary>
    public virtual ICollection<TRoleClaim> Claims { get; set; } = null!;
}
using Microsoft.AspNetCore.Identity;

namespace Library.Models;

public class IdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim> : Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : IdentityRoleClaim<TKey>
    where TUserClaim : Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<TKey>
{
    
}
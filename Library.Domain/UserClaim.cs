using Microsoft.AspNetCore.Identity;

namespace Library.Models;

public class UserClaim<TKey> : IdentityUserClaim<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
    
}
﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public  string? UserName { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public new string? Email { get; set; }
    public DateTime Created { get; set; }
    public string? Password { get; set; }
    
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }
 
}
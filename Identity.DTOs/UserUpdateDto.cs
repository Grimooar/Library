﻿
namespace Identity.DTOs;

public class UserUpdateDto
{

    public string Name { get; set; } = "";
   
    public string LastName { get; set; } = "";
    
    public string Email { get; set; } = "";
    
    public string PhoneNumber { get; set; } = "";
    // Ссылка на роли пользователя

}
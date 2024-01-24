﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClassLibrary1;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Claim = System.Security.Claims.Claim;

namespace Library.Core.Service
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager,IConfiguration configuration,RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public async Task<string?> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByIdAsync(loginDto.Id);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return null;
            }

            // Получение ролей пользователя
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Добавление утверждений ролей
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Создание JWT токена
            var authOptions = _configuration.GetSection("AuthOptions").Get<AuthOptions>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(authOptions.Issuer, authOptions.Audience, claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(authOptions.Lifetime)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
        // public async Task<string?> Login(LoginDto loginDto)
        // {
        //    //var user = await _userService.GetUserByEmailAsync(loginDto.Email);
        //      var user = await _userManager.FindByEmailAsync(loginDto.Email);
        //     if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        //         return null;
        //
        //     var claims = new List<Claim>
        //     {
        //         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //         new Claim(ClaimTypes.Email, user.Email),
        //         new Claim(ClaimTypes.Name, user.UserName)
        //     };
        //
        //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Key));
        //     var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //     var token = new JwtSecurityToken(_authOptions.Issuer, _authOptions.Audience, claims,
        //         expires: DateTime.Now.AddMinutes(Convert.ToDouble(_authOptions.Lifetime)), signingCredentials: credentials);
        //
        //     return new JwtSecurityTokenHandler().WriteToken(token);
        // }


        public async Task<IdentityResult> Register(UserDto userDto, string[] roles = null)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Created = DateTime.UtcNow,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            // Назначение ролей пользователю
            if (result.Succeeded && roles != null && roles.Any())
            {
                await _userManager.AddToRolesAsync(user, roles);
            }

            return result;
        }
        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole<int>(roleName);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _roleManager.RoleExistsAsync(roleName))
            {
                return await _userManager.AddToRoleAsync(user, roleName);
            }

            return IdentityResult.Failed(new IdentityError { Description = "User not found or role does not exist." });
        }

        public async Task<IdentityResult> CreatePermissionAsync(string roleName, string permission)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var claim = new Claim("Permission", permission);
                return await _roleManager.AddClaimAsync(role, claim);
            }

            return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
        }


    }
}

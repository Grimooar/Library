using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ClassLibrary1;
using Library.Core.Service;
using Library.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthOptions _authOptions;
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public AuthController(IOptions<AuthOptions> authOptions, UserService userService, AuthService authService)
        {
            _authOptions = authOptions.Value;
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto userDto)
        {
            var result = await _authService.Register(userDto);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("createPermission")]
        public async Task<IActionResult> CreatePermission(string roleName, string permission)
        {
            var result = await _authService.CreatePermissionAsync(roleName, permission);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
        {
            var result = await _authService.AssignRoleToUserAsync(userId, roleName);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        
        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _authService.CreateRoleAsync(roleName);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _authService.Login(loginDto);

            if (token == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            return Ok(new { token });
        }
    }
}

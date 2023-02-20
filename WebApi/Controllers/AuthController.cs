using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Service;

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
        public async Task<IActionResult> Register(UserDto userDto)
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

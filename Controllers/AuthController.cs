using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Exceptions;
using LearningManagementSystemApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] AuthRegisterRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthRegisterResponseDto responseDto = await _authService.Register(requestDto);
            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtResponseDto>> Login([FromBody] AuthLoginRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Models.AppUser appUser = await _authService.ValidateUser(requestDto);
            if (appUser == null)
            {
                throw new UserNotFoundException("Invalid Email or Password, User not found");
            }

            string token = _jwtService.GenerateToken(new[]
            {
                new Claim(ClaimTypes.Name, appUser.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, appUser.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString())
            });

            return new JwtResponseDto { Token = token };

        }


    }
}

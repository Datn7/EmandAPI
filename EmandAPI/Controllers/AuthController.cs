using Microsoft.AspNetCore.Mvc;
using EmandAPI.Models.DTOs;
using EmandAPI.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Claim = System.Security.Claims.Claim;

namespace EmandAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IValidator<RegisterDTO> _validator;

        public AuthController(UserManager<User> userManager, IMapper mapper, IConfiguration config, IValidator<RegisterDTO> validator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var user = _mapper.Map<User>(dto);
            user.UserName = dto.Email;
            user.Email = dto.Email;

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                message = "User registered successfully",
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Address,
                    user.ProfilePictureUrl,
                    user.Latitude,
                    user.Longitude
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized("Invalid credentials");

                var claims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FullName", user.FullName ?? "")
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

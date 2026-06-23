using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using northwindreactapi.Models.Dto;
using northwindreactapi.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text; 
namespace northwindreactapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
         
        private readonly string secretKey;

        public AuthController(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret") ?? "";

        }
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User deleted successfully");
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            IdentityUser identityUser = new()
            {
                UserName = model.UserName,

            };
            var result = await _userManager.CreateAsync(identityUser, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(identityUser, SD.Role_Customer);

            var response = new RegisterResponseDTO()
            {
                UserName = model.UserName,
                Message = "Created successfully"
            };
            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null ||
                !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    [
                    new (ClaimTypes.Name,user.UserName ),
                                new (ClaimTypes.NameIdentifier,user.Id),
                                new (ClaimTypes.Email,user.Email  ??""),
                                new (ClaimTypes.Role,role )
                    ]),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
             
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new LoginResponseDTO
            {
                UserName = user.UserName  ,
                Email = user.Email  ,
                Token = jwtToken
            });
        }









    }
}

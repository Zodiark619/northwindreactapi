using exerciseAPI.Models;
using exerciseAPI.Models.Dto;
using exerciseAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace exerciseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly string secretKey;
        private readonly ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 

        public AuthController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret") ?? "";
            _response = new ApiResponse();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new()
                {
                    Email = registerRequestDTO.Email,
                    NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                    Name = registerRequestDTO.Name,
                    UserName = registerRequestDTO.Name
                };
                var result = await _userManager.CreateAsync(newUser, registerRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));

                    }
                    if (registerRequestDTO.Role.Equals(SD.Role_Admin, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);

                    }
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);

                }
                else
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    foreach (var error in result.Errors)
                    {

                        _response.ErrorMessages.Add(error.Description);

                    }
                    return BadRequest(_response);
                }
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                foreach (var error in ModelState.Values)
                {
                    foreach (var item in error.Errors)
                    {
                        _response.ErrorMessages.Add(item.ErrorMessage);
                    }
                }
                return BadRequest(_response);
            }


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (ModelState.IsValid)
            {
                 
                var userFromDb = await _userManager.FindByEmailAsync(loginRequestDTO.Email );

                if(userFromDb != null)
                {
                    bool isValid=await _userManager.CheckPasswordAsync(userFromDb,loginRequestDTO.Password);
                    if (!isValid)
                    {
                        _response.Result = new LoginResponseDTO();
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("invlaid credential");
                        return BadRequest(_response);
                    }
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    byte[] key = Encoding.ASCII.GetBytes(secretKey);
                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Subject=new ClaimsIdentity([
                            new ("fullname",userFromDb.Name),
                            new ("id",userFromDb.Name),
                            new ( ClaimTypes.Email,userFromDb.Email!.ToString()),
                            new ( ClaimTypes.Role,_userManager.GetRolesAsync(userFromDb).Result.FirstOrDefault()! ),


                            ]),
                        Expires=DateTime.UtcNow.AddDays(7),
                        SigningCredentials=new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                    };
                    SecurityToken token=tokenHandler.CreateToken(tokenDescriptor);
                    LoginResponseDTO loginResponse = new LoginResponseDTO()
                    {
                        Token=tokenHandler.WriteToken(token),
                        Email=userFromDb.Email,
                        Role= _userManager.GetRolesAsync(userFromDb).Result.FirstOrDefault()!
                    };
                    _response.Result = loginResponse;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                     return Ok(loginResponse);


                }

                _response.Result = new LoginResponseDTO();
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("invlaid credential");
                return BadRequest(_response);


            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                foreach (var error in ModelState.Values)
                {
                    foreach (var item in error.Errors)
                    {
                        _response.ErrorMessages.Add(item.ErrorMessage);
                    }
                }
                return BadRequest(_response);
            }


        }
    }
}

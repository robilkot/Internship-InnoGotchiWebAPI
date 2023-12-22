using AutoMapper;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using InnoGotchiWebAPI.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InnoGotchiWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("token")]
        public async Task<ActionResult<string>> Login([FromForm] string login, [FromForm] string password, IOptions<LoginOptions> loginOptions, [FromServices] IConfiguration configuration)
        {
            var clientUser = await _loginService.Login(login, password);

            var token = GenerateToken(clientUser, loginOptions, configuration);

            Log.Information("User authenticated with login {@login}", clientUser.Login);

            return token;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] string login, [FromForm] string password, [FromForm] string? nickname = default)
        {
            await _loginService.Register(login, password, nickname);

            Log.Information("User registered with login {@login}", login);

            return Ok();
        }

        [NonAction]
        private static string GenerateToken(DbUserModel clientUser, IOptions<LoginOptions> loginOptions, IConfiguration configuration)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, clientUser.Login),
                new Claim(ClaimTypes.Role, clientUser.Role!),
                new Claim("Nickname", clientUser.Nickname!)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["innogotchi:tokensecretkey"]!));

            var jwt = new JwtSecurityToken(
                    claims: claims,
                    issuer: loginOptions.Value.TokenIssuer,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(loginOptions.Value.TokenLifeTime)),
                    signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }
    }
}
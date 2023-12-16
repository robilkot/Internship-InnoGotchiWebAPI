using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiWebAPI.Controllers
{
    [ApiController]
    [Route("innogotchi")]
    public class InnoGotchiLoginController : ControllerBase
    {
        private readonly IInnoGotchiLoginService _loginService;

        public InnoGotchiLoginController(IInnoGotchiLoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ClientUserModel>> Login([FromHeader] string login, [FromHeader] string password)
        {
            return await _loginService.Login(login, password);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromHeader] string login, [FromHeader] string password, [FromHeader] string? nickname = default)
        {
            await _loginService.Register(login, password, nickname);
            return Ok();
        }
    }
}
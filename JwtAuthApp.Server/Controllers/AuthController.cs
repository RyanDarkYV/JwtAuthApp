using JwtAuthApp.Server.Auth;
using JwtAuthApp.Server.Helpers;
using JwtAuthApp.Server.Models;
using JwtAuthApp.Server.Models.Identity;
using JwtAuthApp.Server.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace JwtAuthApp.Server.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtIssuerOptions)
        {
            _userManager = userManager;
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            _jwtFactory = jwtFactory;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "invalid username or password.",
                    ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtIssuerOptions,
                new JsonSerializerSettings {Formatting = Formatting.Indented});

            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string credentialsUserName, string credentialsPassword)
        {
            if (string.IsNullOrEmpty(credentialsUserName) || string.IsNullOrEmpty(credentialsPassword))
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            var userToVerify = await _userManager.FindByNameAsync(credentialsUserName);
            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);


            if (await  _userManager.CheckPasswordAsync(userToVerify, credentialsPassword))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(credentialsUserName, userToVerify.Id));
            }

            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
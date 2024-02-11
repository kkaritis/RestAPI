using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Dtos;
using TodoAPI.Services;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // POST: api/authentication/login
        // Authenticates a user with email and password
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Authenticate(UserLoginDto userLoginParams)
        {
            var authenticationToken = await _authenticationService.Authenticate(userLoginParams);

            if (authenticationToken == null)
            {
                return Unauthorized();
            }

            return Ok(new { token = authenticationToken });
        }
    }
}
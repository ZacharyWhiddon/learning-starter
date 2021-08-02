using LearningStarterServer.Common;
using LearningStarterServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarterServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(string username, string password)
        {
            var response = new Response();

            var isLoggedIn = _authenticationService.Login(username, password);

            if (!isLoggedIn)
            {
                response.AddError(string.Empty, "Username or password is incorrect");
                return BadRequest(response);
            }

            response.Data = true;

            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _authenticationService.Logout();
            return Ok();
        }

        [Authorize]
        [HttpGet("get-current-user")]
        public IActionResult GetLoggedInUser()
        {
            var response = new Response();

            var user = _authenticationService.GetLoggedInUser();

            if(user == null)
            {
                response.AddError(string.Empty, "There was an issue getting the logged in user.");
                return NotFound(response);
            }

            response.Data = user;

            return Ok(response);
        }
    }
}

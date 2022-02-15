using LearningStarter.Common;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginDto dto)
        {
            var response = new Response();

            var isLoggedIn = _authenticationService.Login(dto.UserName, dto.Password);

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

            if (user == null)
            {
                response.AddError(string.Empty, "There was an issue getting the logged in user.");
                return NotFound(response);
            }

            var userGetDto = new UserGetDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username
            };

            response.Data = userGetDto;

            return Ok(response);
        }
    }

    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
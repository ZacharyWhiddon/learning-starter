using LearningStarterServer.Common;
using LearningStarterServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LearningStarter.Common;
using LearningStarter.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningStarterServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response();
            response.Data = await _context.Users.ToListAsync();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(UserCreateDto userCreateDto)
        {
            var response = new Response();

            if (userCreateDto.FirstName == null || userCreateDto.FirstName == "")
            {
                response.AddError("First Name", "First name cannot be empty.");
            }

            if (userCreateDto.LastName == null || userCreateDto.LastName == "")
            {
                response.AddError("Last Name", "Last name cannot be empty.");
            }

            if (userCreateDto.Username == null || userCreateDto.Username == "")
            {
                response.AddError("User Name", "User name cannot be empty.");
            }

            if (userCreateDto.Password == null || userCreateDto.Password == "")
            {
                response.AddError("Password", "Password cannot be empty.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var userToCreate = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Username = userCreateDto.Username,
                Password = userCreateDto.Password,
            };

            _context.Users.Add(userToCreate);
            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = userToCreate.Id,
                FirstName = userToCreate.FirstName,
                LastName = userToCreate.LastName,
                Username = userToCreate.Username
            };

            response.Data = userGetDto;

            return Created("", response);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if(user == null)
            {
                response.AddError("id", "There was a problem finding the user.");
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

        [HttpPut]
        public IActionResult Edit(int id, UserUpdateDto user)
        {
            var response = new Response();

            if (user == null)
            {
                response.AddError("id", "There was a problem editing the user.");
                return NotFound(response);
            }
            
            var userToEdit = _context.Users.FirstOrDefault(x => x.Id == id);

            if (userToEdit == null)
            {
                response.AddError("id", "Could not find user to edit.");
                return NotFound(response);
            }

            if (user.FirstName == null || user.FirstName == "")
            {
                response.AddError("First Name", "First name cannot be empty.");
            }

            if (user.LastName == null || user.LastName == "")
            {
                response.AddError("Last Name", "Last name cannot be empty.");
            }

            if (user.Username == null || user.Username == "")
            {
                response.AddError("User Name", "User name cannot be empty.");
            }

            if (user.Password == null || user.Password == "")
            {
                response.AddError("Password", "Password cannot be empty.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            userToEdit.FirstName = user.FirstName;
            userToEdit.LastName = user.LastName;
            userToEdit.Username = user.Username;
            userToEdit.Password = user.Password;

            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = userToEdit.Id,
                FirstName = userToEdit.FirstName,
                LastName = userToEdit.LastName,
                Username = userToEdit.Username
            };

            response.Data = userGetDto;

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                response.AddError("id", "There was a problem deleting the user.");
                return NotFound(response);
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            response.Data = true;

            return Ok(response);
        }
    }
}

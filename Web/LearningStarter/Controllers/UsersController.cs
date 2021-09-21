using LearningStarterServer.Common;
using LearningStarterServer.Data;
using LearningStarterServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LearningStarter.Common;
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
        public IActionResult Create(User user)
        {
            var response = new Response();

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

            _context.Users.Add(user);
            _context.SaveChanges();

            response.Data = user;

            return Created("", response);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var response = new Response();

            var user = _context.Users.Find(id);

            if(user == null)
            {
                response.AddError("id", "There was a problem finding the user.");
                return NotFound(response);
            }

            response.Data = user;

            return Ok(response);
        }

        [HttpPut]
        public IActionResult Edit(int id, User user)
        {
            var response = new Response();

            var userToEdit = _context.Users.Find(id);

            if (user == null)
            {
                response.AddError("id", "There was a problem deleting the user.");
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

            response.Data = userToEdit;

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var user = _context.Users.Find(id);

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

﻿using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            response.Data = _context
                .Users
                .Select(x => new UserGetDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Username = x.Username
                })
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(
            [FromRoute] int id)
        {
            var response = new Response();

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
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

        [HttpPost]
        public IActionResult Create(
            [FromBody] UserCreateDto userCreateDto)
        {
            var response = new Response();

            if (userCreateDto.FirstName == null || userCreateDto.FirstName == "")
            {
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (userCreateDto.LastName == null || userCreateDto.LastName == "")
            {
                response.AddError("lastName", "Last name cannot be empty.");
            }

            if (userCreateDto.Username == null || userCreateDto.Username == "")
            {
                response.AddError("userName", "User name cannot be empty.");
            }

            if (userCreateDto.Password == null || userCreateDto.Password == "")
            {
                response.AddError("password", "Password cannot be empty.");
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

        [HttpPut("{id}")]
        public IActionResult Edit(
            [FromRoute] int id, 
            [FromBody] UserUpdateDto user)
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
                response.AddError("firstName", "First name cannot be empty.");
            }

            if (user.LastName == null || user.LastName == "")
            {
                response.AddError("lirstName", "Last name cannot be empty.");
            }

            if (user.Username == null || user.Username == "")
            {
                response.AddError("userName", "User name cannot be empty.");
            }

            if (user.Password == null || user.Password == "")
            {
                response.AddError("password", "Password cannot be empty.");
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

        [HttpDelete("{id}")]
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

            return Ok(response);
        }
    }
}

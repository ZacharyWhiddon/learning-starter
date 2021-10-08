using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LearningStarter.Common;
using LearningStarter.Entities;
using LearningStarterServer.Common;
using LearningStarterServer.Data;
using LearningStarterServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api/classes")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AuthenticationController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response();

            var classesToReturn = await _dataContext.Classes.Include(x => x.User).ToListAsync();
            response.Data = classesToReturn.ToDto();

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ClassDto classToCreate)
        {
            var response = new Response();

            if (classToCreate.UserId == 0)
            {
                response.Errors.Add(new Error("User Id must not be empty.", "UserId"));
            }

            if (classToCreate.Subject == string.Empty)
            {
                response.Errors.Add(new Error("Subject must not be empty", "Subject"));
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            //This is how you manually hydrate entities from a DTO.
            var classToAddToDb = new Class
            {
                Capacity = classToCreate.Capacity,
                Subject = classToCreate.Subject,
                UserId = classToCreate.UserId,
            };

            var classAdded = await _dataContext.Classes.AddAsync(classToAddToDb);
            await _dataContext.SaveChangesAsync();

            if (classAdded.Entity == null)
            {
                return BadRequest(response);
            }

            response.Data = classAdded.Entity;
            return Ok(response);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] ClassDto classToUpdate)
        {
            var response = new Response();

            var classInDb = await _dataContext.Classes.SingleOrDefaultAsync(x => x.Id == classToUpdate.Id);

            if (classInDb == null)
            {
                response.Errors.Add(new Error("Id", "Error finding class."));
            }

            if (classToUpdate.UserId == 0)
            {
                response.Errors.Add(new Error("UserId", "User Id must not be empty."));
            }

            if (classToUpdate.Subject == string.Empty)
            {
                response.Errors.Add(new Error("Subject", "Subject must not be empty"));
            }

            if (response.HasErrors || classInDb == null)
            {
                return BadRequest(response);
            }

            classInDb.Subject = classToUpdate.Subject;
            classInDb.Capacity = classToUpdate.Capacity;
            classInDb.UserId = classToUpdate.UserId;

            response.Data = classInDb;

            await _dataContext.SaveChangesAsync();
            return Ok(response);
        }
    }
}
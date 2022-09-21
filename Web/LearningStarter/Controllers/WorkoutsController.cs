using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/workouts")]
    public class WorkoutsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public WorkoutsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var workouts = _dataContext
                .Workouts
                .Select(workout => new WorkoutGetDto
                {
                    Id = workout.Id,
                    Name = workout.Name,
                    WorkoutTypeName = workout.WorkoutType.Name,
                })
                .ToList();

            response.Data = workouts;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] WorkoutCreateDto workoutDto)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(workoutDto.Name))
            {
                response.AddError("name", "Name cannot be empty");
            }

            if (!_dataContext.WorkoutTypes.Any(x => x.Id == workoutDto.WorkoutTypeId))
            {
                response.AddError("workoutTypeId", "Workout type does not exist.");
            }

            if (_dataContext.Workouts.Any(x => x.Name == workoutDto.Name))
            {
                response.AddError("name", "Workout already exists.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var workoutToCreate = new Workout
            {
                Name = workoutDto.Name,
                WorkoutTypeId = workoutDto.WorkoutTypeId
            };

            _dataContext.Workouts.Add(workoutToCreate);
            _dataContext.SaveChanges();

            var workoutTypeName = _dataContext.WorkoutTypes
                .First(x => x.Id == workoutDto.WorkoutTypeId)
                .Name;

            var workoutToReturn = new WorkoutGetDto
            {
                Id = workoutToCreate.Id,
                Name = workoutToCreate.Name,
                WorkoutTypeName = workoutTypeName
            };

            response.Data = workoutToReturn;
            return Created("", response);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id,
            [FromBody] WorkoutUpdateDto workoutDto)
        {
            var response = new Response();

            var workoutToUpdate = _dataContext.Workouts.First(x => x.Id == id);

            workoutToUpdate.Name = workoutDto.Name;
            workoutToUpdate.WorkoutTypeId = workoutDto.WorkoutTypeId;

            _dataContext.SaveChanges();

            var workoutTypeName = _dataContext.WorkoutTypes
                .First(x => x.Id == workoutDto.WorkoutTypeId)
                .Name;

            var workoutToReturn = new WorkoutGetDto
            {
                Id = workoutToUpdate.Id,
                Name = workoutToUpdate.Name,
                WorkoutTypeName = workoutTypeName
            };

            response.Data = workoutToReturn;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = new Response();

            var workoutToRemove = _dataContext.Workouts.FirstOrDefault(x => x.Id == id);

            if(workoutToRemove == null)
            {
                response.AddError("id", "Workout does not exist");
                return NotFound(response);
            }

            _dataContext.Workouts.Remove(workoutToRemove);
            _dataContext.SaveChanges();

            response.Data = true;
            return Ok(response);
        }
    }

}

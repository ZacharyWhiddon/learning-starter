using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/user-workouts")]
    public class UserWorkoutsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UserWorkoutsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var userWorkouts = _dataContext
                .UserWorkouts
                .Select(userWorkout => new UserWorkoutGetDto
                {
                    Id = userWorkout.Id,
                    User = new UserGetDto
                    {
                        Id = userWorkout.UserId,
                        FirstName = userWorkout.User.FirstName,
                        LastName = userWorkout.User.LastName,
                        Username = userWorkout.User.Username,
                    },
                    Workout = new WorkoutGetDto
                    {
                        Id = userWorkout.WorkoutId,
                        Name = userWorkout.Workout.Name,
                        WorkoutTypeName = userWorkout.Workout.WorkoutType.Name,
                    },
                    WorkoutId = userWorkout.WorkoutId,
                })
                .ToList();

            response.Data = userWorkouts;
            return Ok(response);
        }
    }
}

using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api/preparation-steps")]
    public class PreparationStepsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PreparationStepsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var preparationStepsToReturn = _dataContext
                .PreparationSteps
                .Select(x => new PreparationStepGetDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            response.Data = preparationStepsToReturn;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = new Response();

            var preparationStepsToReturn = _dataContext
                .PreparationSteps
                .Select(x => new PreparationStepGetDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefault(x => x.Id == id);

            response.Data = preparationStepsToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] PreparationStepCreateDto preparationStepCreateDto)
        {
            var response = new Response();

            var preparationStepToCreate = new PreparationStep
            {
                Name = preparationStepCreateDto.Name
            };

            _dataContext.PreparationSteps.Add(preparationStepToCreate);
            _dataContext.SaveChanges();

            var preparationStepGetDto = new PreparationStepGetDto
            {
                Id = preparationStepToCreate.Id,
                Name = preparationStepToCreate.Name
            };

            response.Data = preparationStepGetDto;
            return Ok(response);
        }

        [HttpPut]
        public IActionResult Update(
            [FromBody] PreparationStepUpdateDto preparationStepUpdateDto)
        {
            var response = new Response();

            var preparationStepToUpdate = _dataContext
                .PreparationSteps
                .FirstOrDefault(x => x.Id == preparationStepUpdateDto.Id);

            preparationStepToUpdate.Name = preparationStepUpdateDto.Name;

            _dataContext.SaveChanges();

            var preparationStepGetDto = new PreparationStepGetDto
            {
                Id = preparationStepToUpdate.Id,
                Name = preparationStepToUpdate.Name
            };

            response.Data = preparationStepGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var preparationStepToDelete = _dataContext
                .PreparationSteps
                .FirstOrDefault(x => x.Id == id);

            _dataContext.PreparationSteps.Remove(preparationStepToDelete);
            _dataContext.SaveChanges();

            return Ok();
        }
    }
}
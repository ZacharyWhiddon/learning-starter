using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/product-types")]
    public class ProductTypesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProductTypesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var productTypesToReturn = _dataContext
                .ProductTypes
                .Select(x => new ProductTypeGetDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            response.Data = productTypesToReturn;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(
            [FromRoute] int id)
        {
            var response = new Response();

            var productTypeToReturn = _dataContext
                .ProductTypes
                .Select(x => new ProductTypeGetDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefault(x => x.Id == id);

            if (productTypeToReturn == null)
            {
                response.AddError("id", "Requested Product Type does not exist.");
                return NotFound(response);
            }

            response.Data = productTypeToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] ProductTypeCreateDto productTypeCreateDto)
        {
            var response = new Response();

            if (productTypeCreateDto.Name == null || productTypeCreateDto.Name == "")
            {
                response.AddError("Name", "Name must not be empty.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var productTypeToCreate = new ProductType
            {
                Name = productTypeCreateDto.Name
            };

            _dataContext.ProductTypes.Add(productTypeToCreate);
            _dataContext.SaveChanges();

            var productTypeGetDto = new ProductTypeGetDto
            {
                Id = productTypeToCreate.Id,
                Name = productTypeToCreate.Name
            };

            response.Data = productTypeGetDto;
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] ProductTypeUpdateDto productTypeUpdateDto)
        {
            var response = new Response();

            if (productTypeUpdateDto.Name == null || productTypeUpdateDto.Name == "")
            {
                response.AddError("Name", "Name must not be empty.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var productTypeToUpdate = _dataContext
                .ProductTypes
                .FirstOrDefault(x => x.Id == id);

            if (productTypeToUpdate == null)
            {
                response.AddError("id", "Product Type does not exist.");
                return NotFound(response);
            }

            productTypeToUpdate.Name = productTypeUpdateDto.Name;

            _dataContext.SaveChanges();

            var productTypeGetDto = new ProductTypeGetDto
            {
                Id = productTypeToUpdate.Id,
                Name = productTypeToUpdate.Name
            };

            response.Data = productTypeGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(
            [FromRoute] int id)
        {
            var response = new Response();

            var productTypeToDelete = _dataContext
                .ProductTypes
                .FirstOrDefault(x => x.Id == id);

            if (productTypeToDelete == null)
            {
                response.AddError("id", "Product Type does not exist.");
                return NotFound(response);
            }

            _dataContext.ProductTypes.Remove(productTypeToDelete);
            _dataContext.SaveChanges();

            return Ok(response);
        }
    }
}
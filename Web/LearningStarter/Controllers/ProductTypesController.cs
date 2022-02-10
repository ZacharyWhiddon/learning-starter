using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api/product-types")]
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
        public IActionResult GetById(int id)
        {
            var response = new Response();

            var productTypesToReturn = _dataContext
                .ProductTypes
                .Select(x => new ProductTypeGetDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefault(x => x.Id == id);

            response.Data = productTypesToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] ProductTypeCreateDto productTypeCreateDto)
        {
            var response = new Response();

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

        [HttpPut]
        public IActionResult Update(
            [FromBody] ProductTypeUpdateDto productTypeUpdateDto)
        {
            var response = new Response();

            var productTypeToUpdate = _dataContext
                .ProductTypes
                .FirstOrDefault(x => x.Id == productTypeUpdateDto.Id);

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
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var productTypeToUpdate = _dataContext
                .ProductTypes
                .FirstOrDefault(x => x.Id == id);

            _dataContext.ProductTypes.Remove(productTypeToUpdate);
            _dataContext.SaveChanges();

            return Ok();
        }
    }
}
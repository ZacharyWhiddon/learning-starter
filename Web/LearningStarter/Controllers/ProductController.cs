using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductCreateDto product) 
        {
            var response = new Response();

            var productToCreate = new Product
            {
                Name = product.Name,
                Description = product.Description,
            };

            _dataContext.Set<Product>().Add(productToCreate);
            _dataContext.SaveChanges();

            var productToReturn = new ProductGetDto
            {
                Id = productToCreate.Id,
                Name = productToCreate.Name,
                Description = productToCreate.Description,
            };

            response.Data = productToReturn;
            return Created("", response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext.Set<Product>()
                .Select(x => new ProductGetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                })
                .ToList();
            response.Data = data;
            return Ok(response);
        }
    }
}

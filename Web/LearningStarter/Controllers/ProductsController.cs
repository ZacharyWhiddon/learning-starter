using System;
using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProductsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var productsToReturn = _dataContext
                .Products
                .Select(x => new ProductGetDto
                {
                    Id = x.Id,
                    ProductTypeId = x.ProductTypeId,
                    Name = x.Name,
                    Price = x.Price,
                    ProductTypeName = x.ProductType.Name,
                })
                .ToList();

            response.Data = productsToReturn;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = new Response();

            var productsToReturn = _dataContext
                .Products
                .Select(x => new ProductGetDto
                {
                    Id = x.Id,
                    ProductTypeId = x.ProductTypeId,
                    Name = x.Name,
                    Price = x.Price,
                    ProductTypeName = x.ProductType.Name,
                })
                .FirstOrDefault(x => x.Id == id);

            response.Data = productsToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] ProductCreateDto productCreateDto)
        {
            var response = new Response();

            var productToCreate = new Product
            {
                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                ProductTypeId = productCreateDto.ProductTypeId,
            };

            _dataContext.Products.Add(productToCreate);
            _dataContext.SaveChanges();

            var productGetDto = new ProductGetDto
            {
                Id = productToCreate.Id,
                ProductTypeId = productToCreate.ProductTypeId,
                Name = productToCreate.Name,
                Price = productToCreate.Price,
                ProductTypeName = productToCreate.ProductType.Name,
            };

            response.Data = productGetDto;
            return Ok(response);
        }

        [HttpPut]
        public IActionResult Update(
            [FromBody] ProductUpdateDto productUpdateDto)
        {
            var response = new Response();

            var productToUpdate = _dataContext
                .Products
                .FirstOrDefault(x => x.Id == productUpdateDto.Id);

            productToUpdate.Name = productUpdateDto.Name;
            productToUpdate.Price = productUpdateDto.Price;
            productToUpdate.ProductTypeId = productUpdateDto.ProductTypeId;

            _dataContext.SaveChanges();

            var productGetDto = new ProductGetDto
            {
                Id = productToUpdate.Id,
                ProductTypeId = productToUpdate.ProductTypeId,
                Name = productToUpdate.Name,
                Price = productToUpdate.Price,
                ProductTypeName = productToUpdate.ProductType.Name,
            };

            response.Data = productGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var productToDelete = _dataContext
                .Products
                .FirstOrDefault(x => x.Id == id);

            var orderProductsToDelete = _dataContext
                .OrderProducts
                .Where(x => x.OrderId == id)
                .ToList();

            _dataContext.OrderProducts.RemoveRange(orderProductsToDelete);
            _dataContext.Products.Remove(productToDelete);
            _dataContext.SaveChanges();

            return Ok();
        }
    }
}
using System;
using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api/order-products")]
    public class OrderProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrderProductsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var orderProductsToReturn = _dataContext
                .OrderProducts
                .Select(x => new OrderProductGetDto
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductPrice = x.Product.Price,
                    UserFirstName = x.Order.User.FirstName,
                    UserLastName = x.Order.User.LastName,
                })
                .ToList();

            response.Data = orderProductsToReturn;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = new Response();

            var orderProductsToReturn = _dataContext
                .OrderProducts
                .Select(x => new OrderProductGetDto
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductPrice = x.Product.Price,
                    UserFirstName = x.Order.User.FirstName,
                    UserLastName = x.Order.User.LastName,
                })
                .FirstOrDefault(x => x.Id == id);

            response.Data = orderProductsToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] OrderProductsCreateDto orderProductCreateDto)
        {
            var response = new Response();

            var orderProductToCreate = new OrderProducts
            {
                UserId = orderProductCreateDto.UserId,
                PreparationStepId = orderProductCreateDto.PreparationStepId,
                CreatedDate = DateTimeOffset.Now,
            };

            _dataContext.OrderProducts.Add(orderProductToCreate);
            _dataContext.SaveChanges();

            var orderGetDto = new OrderProductGetDto
            {
                Id = orderProductToCreate.Id,
                OrderId = orderProductToCreate.OrderId,
                ProductId = orderProductToCreate.ProductId,
                ProductName = orderProductToCreate.Product.Name,
                ProductPrice = orderProductToCreate.Product.Price,
                UserFirstName = orderProductToCreate.Order.User.FirstName,
                UserLastName = orderProductToCreate.Order.User.LastName,
            };

            response.Data = orderGetDto;
            return Ok(response);
        }

        [HttpPut]
        public IActionResult Update(
            [FromBody] OrderProductUpdateDto orderProductUpdateDto)
        {
            var response = new Response();

            var orderProductToUpdate = _dataContext
                .OrderProducts
                .FirstOrDefault(x => x.Id == orderProductUpdateDto.Id);

            orderProductToUpdate.PreparationStepId = orderProductUpdateDto.PreparationStepId;

            _dataContext.SaveChanges();

            var orderGetDto = new OrderProductGetDto
            {
                Id = orderProductToUpdate.Id,
                OrderId = orderProductToUpdate.OrderId,
                ProductId = orderProductToUpdate.ProductId,
                ProductName = orderProductToUpdate.Product.Name,
                ProductPrice = orderProductToUpdate.Product.Price,
                UserFirstName = orderProductToUpdate.Order.User.FirstName,
                UserLastName = orderProductToUpdate.Order.User.LastName,
            };

            response.Data = orderGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var orderProductToDelete = _dataContext
                .OrderProducts
                .FirstOrDefault(x => x.Id == id);

            _dataContext.OrderProducts.Remove(orderProductToDelete);
            _dataContext.SaveChanges();

            return Ok();
        }
    }
}
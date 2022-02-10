using System;
using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("/api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrdersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var ordersToReturn = _dataContext
                .Orders
                .Select(x => new OrderGetDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    PreparationStepId = x.PreparationStepId,
                    PreparationStepName = x.PreparationStep.Name,
                    UserFirstName = x.User.FirstName,
                    UserLastName = x.User.LastName,
                    CreatedDate = x.CreatedDate
                })
                .ToList();

            response.Data = ordersToReturn;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = new Response();

            var ordersToReturn = _dataContext
                .Orders
                .Select(x => new OrderGetDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    PreparationStepId = x.PreparationStepId,
                    PreparationStepName = x.PreparationStep.Name,
                    UserFirstName = x.User.FirstName,
                    UserLastName = x.User.LastName,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefault(x => x.Id == id);

            response.Data = ordersToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] OrderCreateDto orderCreateDto)
        {
            var response = new Response();

            var orderToCreate = new Order
            {
                UserId = orderCreateDto.UserId,
                PreparationStepId = orderCreateDto.PreparationStepId,
                CreatedDate = DateTimeOffset.Now,
            };

            _dataContext.Orders.Add(orderToCreate);
            _dataContext.SaveChanges();

            var orderGetDto = new OrderGetDto
            {
                Id = orderToCreate.Id,
                UserId = orderToCreate.UserId,
                PreparationStepId = orderToCreate.PreparationStepId,
                PreparationStepName = orderToCreate.PreparationStep.Name,
                UserFirstName = orderToCreate.User.FirstName,
                UserLastName = orderToCreate.User.LastName,
                CreatedDate = orderToCreate.CreatedDate
            };

            response.Data = orderGetDto;
            return Ok(response);
        }

        [HttpPut]
        public IActionResult Update(
            [FromBody] OrderUpdateDto orderUpdateDto)
        {
            var response = new Response();

            var orderToUpdate = _dataContext
                .Orders
                .FirstOrDefault(x => x.Id == orderUpdateDto.Id);

            orderToUpdate.PreparationStepId = orderUpdateDto.PreparationStepId;

            _dataContext.SaveChanges();

            var orderGetDto = new OrderGetDto
            {
                Id = orderToUpdate.Id,
                UserId = orderToUpdate.UserId,
                PreparationStepId = orderToUpdate.PreparationStepId,
                PreparationStepName = orderToUpdate.PreparationStep.Name,
                UserFirstName = orderToUpdate.User.FirstName,
                UserLastName = orderToUpdate.User.LastName,
                CreatedDate = orderToUpdate.CreatedDate
            };

            response.Data = orderGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var orderToDelete = _dataContext
                .Orders
                .FirstOrDefault(x => x.Id == id);

            var orderProductsToDelete = _dataContext
                .OrderProducts
                .Where(x => x.OrderId == id)
                .ToList();

            _dataContext.OrderProducts.RemoveRange(orderProductsToDelete);
            _dataContext.Orders.Remove(orderToDelete);
            _dataContext.SaveChanges();

            return Ok();
        }
    }
}
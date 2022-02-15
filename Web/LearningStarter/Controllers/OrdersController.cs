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
        public IActionResult GetById(
            [FromRoute] int id)
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

            if (ordersToReturn == null)
            {
                response.AddError("id", "Order not found.");
                return NotFound(response);
            }

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

            var preparationStep = _dataContext
                .Set<PreparationStep>()
                .FirstOrDefault(x => x.Id == orderToCreate.PreparationStepId);

            if (preparationStep == null)
            {
                response.AddError("preparationStepId", "Preparation Step not found.");
                return NotFound(response);
            }

            var user = _dataContext
                .Set<User>()
                .FirstOrDefault(x => x.Id == orderToCreate.UserId);

            if (user == null)
            {
                response.AddError("userId", "User not found.");
                return NotFound(response);
            }

            _dataContext.Orders.Add(orderToCreate);
            _dataContext.SaveChanges();

            var orderGetDto = new OrderGetDto
            {
                Id = orderToCreate.Id,
                UserId = orderToCreate.UserId,
                PreparationStepId = orderToCreate.PreparationStepId,
                PreparationStepName = preparationStep.Name,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                CreatedDate = orderToCreate.CreatedDate
            };

            response.Data = orderGetDto;
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] OrderUpdateDto orderUpdateDto)
        {
            var response = new Response();

            var orderToUpdate = _dataContext
                .Orders
                .FirstOrDefault(x => x.Id == id);

            if (orderToUpdate == null)
            {
                response.AddError("id", "Order not found.");
                return NotFound(response);
            }

            orderToUpdate.PreparationStepId = orderUpdateDto.PreparationStepId;

            var preparationStep = _dataContext
                .Set<PreparationStep>()
                .FirstOrDefault(x => x.Id == orderToUpdate.PreparationStepId);

            if (preparationStep == null)
            {
                response.AddError("preparationStepId", "Preparation Step not found.");
                return NotFound(response);
            }

            var user = _dataContext
                .Set<User>()
                .FirstOrDefault(x => x.Id == orderToUpdate.UserId);

            if (user == null)
            {
                response.AddError("userId", "User not found.");
                return NotFound(response);
            }

            _dataContext.SaveChanges();

            var orderGetDto = new OrderGetDto
            {
                Id = orderToUpdate.Id,
                UserId = orderToUpdate.UserId,
                PreparationStepId = orderToUpdate.PreparationStepId,
                PreparationStepName = preparationStep.Name,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                CreatedDate = orderToUpdate.CreatedDate
            };

            response.Data = orderGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(
            [FromRoute] int id)
        {
            var response = new Response();

            var orderToDelete = _dataContext
                .Orders
                .FirstOrDefault(x => x.Id == id);

            if (orderToDelete == null)
            {
                return Ok();
            }

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
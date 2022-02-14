using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using LearningStarterServer.Entities;
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
        public IActionResult GetById(
            [FromRoute] int id)
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
            [FromBody] OrderProductCreateDto orderProductCreateDto)
        {
            var response = new Response();

            var orderProductToCreate = new OrderProduct
            {
                OrderId = orderProductCreateDto.OrderId,
                ProductId = orderProductCreateDto.ProductId,
            };

            _dataContext.OrderProducts.Add(orderProductToCreate);
            _dataContext.SaveChanges();

            var product = _dataContext
                .Set<Product>()
                .FirstOrDefault(x => x.Id == orderProductToCreate.ProductId);

            if (product == null)
            {
                response.AddError("ProductId", "Product not found.");
                return NotFound(response);
            }

            var order = _dataContext
                .Set<Order>()
                .FirstOrDefault(x => x.Id == orderProductToCreate.OrderId);

            if (order == null)
            {
                response.AddError("OrderId", "Order not found.");
                return NotFound(response);
            }

            var user = _dataContext
                .Set<User>()
                .FirstOrDefault(x => x.Id == order.UserId);

            if (user == null)
            {
                response.AddError("User", "User for this Order not found.");
                return NotFound(response);
            }

            var orderProductGetDto = new OrderProductGetDto
            {
                Id = orderProductToCreate.Id,
                OrderId = orderProductToCreate.OrderId,
                ProductId = orderProductToCreate.ProductId,
                ProductName = product.Name,
                ProductPrice = product.Price,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
            };

            response.Data = orderProductGetDto;
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] OrderProductUpdateDto orderProductUpdateDto)
        {
            var response = new Response();

            var orderProductToUpdate = _dataContext
                .OrderProducts
                .FirstOrDefault(x => x.Id == id);

            if (orderProductToUpdate == null)
            {
                response.AddError("id", "Order Product not found.");
                return NotFound(response);
            }

            orderProductToUpdate.OrderId = orderProductUpdateDto.OrderId;
            orderProductToUpdate.ProductId = orderProductUpdateDto.ProductId;

            _dataContext.SaveChanges();

            var product = _dataContext
                .Set<Product>()
                .FirstOrDefault(x => x.Id == orderProductUpdateDto.ProductId);

            if (product == null)
            {
                response.AddError("ProductId", "Product not found.");
                return NotFound(response);
            }

            var order = _dataContext
                .Set<Order>()
                .FirstOrDefault(x => x.Id == orderProductUpdateDto.OrderId);

            if (order == null)
            {
                response.AddError("OrderId", "Order not found.");
                return NotFound(response);
            }

            var user = _dataContext
                .Set<User>()
                .FirstOrDefault(x => x.Id == order.UserId);

            if (user == null)
            {
                response.AddError("User", "User for this Order not found.");
                return NotFound(response);
            }

            var orderProductGetDto = new OrderProductGetDto
            {
                Id = orderProductToUpdate.Id,
                OrderId = orderProductToUpdate.OrderId,
                ProductId = orderProductToUpdate.ProductId,
                ProductName = product.Name,
                ProductPrice = product.Price,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
            };

            response.Data = orderProductGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var orderProductToDelete = _dataContext
                .OrderProducts
                .FirstOrDefault(x => x.Id == id);

            if (orderProductToDelete == null)
            {
                return Ok();
            }

            _dataContext.OrderProducts.Remove(orderProductToDelete);
            _dataContext.SaveChanges();

            return Ok();
        }
    }
}
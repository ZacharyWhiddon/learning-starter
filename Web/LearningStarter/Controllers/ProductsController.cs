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
        public IActionResult GetById(
            [FromRoute] int id)
        {
            var response = new Response();

            var productToReturn = _dataContext
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

            if (productToReturn == null)
            {
                response.AddError("id", "Product not found.");
                return NotFound(response);
            }

            response.Data = productToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] ProductCreateDto productCreateDto)
        {
            var response = new Response();

            if (productCreateDto.Name == null || productCreateDto.Name.Trim() == "")
            {
                response.AddError("Name", "Name cannot be empty.");
            }

            if (productCreateDto.Price > 0)
            {
                response.AddError("Price", "Price must be greater than 0.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var productToCreate = new Product
            {
                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                ProductTypeId = productCreateDto.ProductTypeId,
            };

            var productType = _dataContext
                .Set<ProductType>()
                .FirstOrDefault(x => x.Id == productToCreate.ProductTypeId);

            if (productType == null)
            {
                response.AddError("ProductTypeId", "Product Type not found.");
                return NotFound(response);
            }

            _dataContext.Products.Add(productToCreate);
            _dataContext.SaveChanges();

            var productGetDto = new ProductGetDto
            {
                Id = productToCreate.Id,
                ProductTypeId = productToCreate.ProductTypeId,
                Name = productToCreate.Name,
                Price = productToCreate.Price,
                ProductTypeName = productType.Name,
            };

            response.Data = productGetDto;
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] ProductUpdateDto productUpdateDto)
        {
            var response = new Response();

            if (productUpdateDto.Name == null || productUpdateDto.Name.Trim() == "")
            {
                response.AddError("Name", "Name cannot be empty.");
            }

            if (productUpdateDto.Price > 0)
            {
                response.AddError("Price", "Price must be greater than 0.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var productToUpdate = _dataContext
                .Products
                .FirstOrDefault(x => x.Id == id);

            if (productToUpdate == null)
            {
                response.AddError("id", "Product not found.");
                return NotFound(response);
            }

            productToUpdate.Name = productUpdateDto.Name;
            productToUpdate.Price = productUpdateDto.Price;
            productToUpdate.ProductTypeId = productUpdateDto.ProductTypeId;

            var productType = _dataContext
                .Set<ProductType>()
                .FirstOrDefault(x => x.Id == productToUpdate.ProductTypeId);

            if (productType == null)
            {
                response.AddError("id", "Product Type not found.");
                return NotFound(response);
            }

            _dataContext.SaveChanges();

            var productGetDto = new ProductGetDto
            {
                Id = productToUpdate.Id,
                ProductTypeId = productToUpdate.ProductTypeId,
                Name = productToUpdate.Name,
                Price = productToUpdate.Price,
                ProductTypeName = productType.Name,
            };

            response.Data = productGetDto;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(
            [FromRoute] int id)
        {
            var response = new Response();

            var productToDelete = _dataContext
                .Products
                .FirstOrDefault(x => x.Id == id);

            if (productToDelete == null)
            {
                return Ok();
            }

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
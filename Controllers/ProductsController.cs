using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefactorThis.ActionFilter;
using RefactorThis.Dtos;
using RefactorThis.Extensions;
using RefactorThis.Models;
using RefactorThis.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RecordLastLoginAttribute))]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly IProductOptionService _productOptionService;
        public ProductsController(ILogger<ProductsController> logger, IProductService productService, IProductOptionService productOptionService)
        {
            _logger = logger;
            _productService = productService;
            _productOptionService = productOptionService;

        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] string name, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderFunct = x => x.OrderBy(e => e.Name);
            PaginatedResultDto<Product> result;
            if (name == null)
            {
                result = await _productService.GetEntitiesAsync(p => true, pageIndex, pageSize, orderFunct);
            }
            else
            {
                result = await _productService.GetEntitiesAsync(p => p.Name.Equals(name), pageIndex, pageSize, orderFunct);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductDto productDto)
        {
            var newProduct = productDto.ProductToModel();
            try
            {
                var product = await _productService.AddAsync(newProduct);
                return Created(Url.Action(Request.BuildUrlActionContext("Products", "Get", new { id = product.Id })), product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during add product");
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProductDto productDto)
        {
            var updateProduct = await FindProduct(id);
            if (updateProduct == null)
            {
                return NotFound();
            }
            try
            {
                updateProduct = updateProduct.AssignDtoToProduct(productDto);
                await _productService.UpdateAsync(updateProduct);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during update product (ID:{id})");
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            try
            {
                await _productService.DeleteProductAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"The delete product ({id}) have an issue to deleted");
                return BadRequest(ex);
            }
            return NoContent();
        }

        [HttpGet("{productId}/options")]
        public async Task<IActionResult> GetOptions(Guid productId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            var result = await _productOptionService.GetEntitiesAsync(p => p.ProductId.Equals(productId), pageIndex, pageSize, po => po.OrderBy(o => o.Id));
            return Ok(result);
        }

        [HttpGet("{productId}/options/{optionId}")]
        public async Task<IActionResult> GetProductOption(Guid productId, Guid optionId)
        {
            var productOption = await FindProductOption(productId, optionId);
            if (productOption == null)
            {
                return NotFound();
            }
            return Ok(productOption);
        }

        [HttpPost("{productId}/options")]
        public async Task<IActionResult> CreateOption(Guid productId, CreateProductOptionDto option)
        {
            var product = await FindProduct(productId);
            if (product == null)
            {
                return NotFound();
            }
            var newProductOption = new ProductOption()
            {
                ProductId = productId,
                Name = option.Name,
                Description = option.Description
            };
            try
            {
                var result = await _productOptionService.AddAsync(newProductOption);
                return Created(Url.Action(Request.BuildUrlActionContext("Products", "Get", new { productId = result.ProductId, productOptionId = result.Id })), result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during create product option. the product ID is {productId}");
                return BadRequest(ex);
            }
           
        }


        [HttpPut("{productId}/options/{optionId}")]
        public async Task<IActionResult> UpdateOption(Guid productId, Guid optionId, ProductOptionDto productOptionDto)
        {
            var productOption = await FindProductOption(productId, optionId);
            if (productOption == null)
            {
                return NotFound();
            }
            // If the updated Product Option's product was not found, it should return not found
            var toProduct = await FindProduct(productOptionDto.ProductId);
            if (toProduct == null)
            {
                return NotFound();
            }
            try
            {
                await _productOptionService.UpdateAsync(productOption.AssignDtoToProductOption(productOptionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"The ProductOption ({optionId}) have an issue to be updated");
                return BadRequest(ex);
            }
            return Ok();
        }

        [HttpDelete("{productId}/options/{optionId}")]
        public async Task<IActionResult> DeleteOption(Guid productId, Guid optionId)
        {
            var productOption = await FindProductOption(productId, optionId);
            if (productOption == null)
            {
                return NotFound();
            }
            try
            {
                await _productOptionService.DeleteAsync(productOption);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"The ProductOption ({optionId}) have an issue to be deleted");
                return BadRequest(ex);
            }
            return NoContent();
        }

        private async Task<Product> FindProduct(Guid productId)
        {
            var product = await _productService.GetEntityAsync(p => p.Id.Equals(productId));
            if (product == null)
            {
                _logger.LogWarning($"The Product (ID: {productId}) was not found");
                return null;
            }
            return product;
        }

        private async Task<ProductOption> FindProductOption(Guid productId, Guid productOptionId)
        {
            var product = await FindProduct(productId);
            if (product == null)
            {
                return null;
            }
            var productOption = await _productOptionService.GetEntityAsync(po => po.ProductId.Equals(productId) && po.Id.Equals(productOptionId));
            if (productOption == null)
            {
                _logger.LogWarning($"The ProductOption (Id: {productOptionId}) is not found with a Product ({productId})");
                return null;
            }
            return productOption;
        }
    }
}
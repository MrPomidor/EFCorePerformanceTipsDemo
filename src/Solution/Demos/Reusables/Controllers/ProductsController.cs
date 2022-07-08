using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reusables.Exceptions;
using Reusables.Filters;
using Reusables.Models;
using Reusables.Repositories;

namespace Reusables.Controllers
{
    [ServiceFilter(typeof(TaskCancelledExceptionFilterAttribute))]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProduct(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _productsRepository.GetProduct(productId, cancellationToken);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("{productId:int}/full")]
        public async Task<IActionResult> GetProductFull(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _productsRepository.GetProductFull(productId, cancellationToken);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPut("{productId:int}")]
        public async Task<IActionResult> EditProductName(int productId, [FromBody] string productName)
        {
            try
            {
                await _productsRepository.EditProductName(productId, productName);
            }
            catch (ProductNotFoundException)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductModel addModel)
        {
            var newProductId = await _productsRepository.CreateProduct(addModel);
            return Created($"api/products/{newProductId}", newProductId);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var productsPage = await _productsRepository.GetProductsPage(page: page, pageSize: pageSize, cancellationToken);
            return Ok(productsPage);
        }

        [HttpGet("list/full")]
        public async Task<IActionResult> GetPageFull([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var productsPage = await _productsRepository.GetProductsPageFull(page: page, pageSize: pageSize, cancellationToken);
            return Ok(productsPage);
        }

        [HttpGet("list/total")]
        public async Task<IActionResult> GetProductsCount(CancellationToken cancellationToken = default)
        {
            var productsCount = await _productsRepository.GetTotalProducts(cancellationToken);
            return Ok(productsCount);
        }
    }
}

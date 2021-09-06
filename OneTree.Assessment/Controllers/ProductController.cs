using Microsoft.AspNetCore.Mvc;
using OneTree.Assessment.Core.Dtos;
using OneTree.Assessment.Core.IService;
using System;
using System.Threading.Tasks;

namespace OneTree.Assessment.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        /// <summary>
        /// service's instance
        /// </summary>
        private readonly IProductService service;

        /// <summary>
        /// Constructor's method
        /// </summary>
        /// <param name="service"></param>
        public ProductController(IProductService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(Product))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await service.GetAllAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ProductToUpdate))]
        public async Task<IActionResult> Create([FromForm] ProductToCreate productCreate)
        {
            return Ok(await service.CreateAsync(productCreate).ConfigureAwait(false));
        }

        /// <summary>
        /// Modify product
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(ProductToUpdate))]
        public async Task<IActionResult> Modify([FromForm] ProductToUpdate productCreateUpdate)
        {
            return Ok(await service.ModifyAsync(productCreateUpdate).ConfigureAwait(false));
        }

        /// <summary>
        /// Remove product
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces(typeof(ProductToUpdate))]
        public async Task<IActionResult> Remove(Guid id)
        {
            await service.RemoveAsync(id).ConfigureAwait(false);
            return Ok();
        }
    }
}

using Application.UseCases.Products.Command;
using Application.UseCases.Products.Query.AllProducts;
using Application.UseCases.Products.Query.OwnProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ApiControllerBase
    {
       private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProductAsync(
            [FromBody] CreateProductCommand createProductCommand)
        {
            var result = await this.mediator.Send(createProductCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOwnProducts()
        {
          return await this.mediator.Send(new GetAllOwnProductsQuery());
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOwnProductByIdAsync([FromQuery] GetOwnProductByIdQuery getProductByIdQuery)
        {
            return await this.mediator.Send(getProductByIdQuery);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            return await this.mediator.Send(new GetAllProductsQuery());
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductByIdAsync([FromBody]  UpdateProductCommand updateProductCommand)
        {
            return await this.mediator.Send(updateProductCommand);
        }
        
        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductByIdAsync([FromQuery] DeleteProductCommand deleteProductByIdQuery)
        {
            return await this.mediator.Send(deleteProductByIdQuery);
        }
    }
}

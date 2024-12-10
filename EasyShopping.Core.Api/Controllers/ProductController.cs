using Application.UseCases.Products.Command;
using Application.UseCases.Products.Query;
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
        public async Task<IActionResult> GetUserProducts()
        {
          return await this.mediator.Send(new GetAllProductsQuery());
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetProductByIdAsync([FromQuery] GetProductByIdQuery getProductByIdQuery)
        {
            return await this.mediator.Send(getProductByIdQuery);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateProductByIdAsync([FromBody]  UpdateProductCommand updateProductCommand)
        {
            return await this.mediator.Send(updateProductCommand);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProductByIdAsync([FromQuery] DeleteProductCommand deleteProductByIdQuery)
        {
            return await this.mediator.Send(deleteProductByIdQuery);
        }
        
    }
}

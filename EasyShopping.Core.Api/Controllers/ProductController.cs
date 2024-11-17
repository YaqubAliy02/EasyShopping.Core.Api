using Application.UseCases.Products.Command;
using MediatR;
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
        public async Task<IActionResult> CreateProductAsync(
            [FromBody] CreateProductCommand createProductCommand)
        {
            var result = await this.mediator.Send(createProductCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }
    }
}

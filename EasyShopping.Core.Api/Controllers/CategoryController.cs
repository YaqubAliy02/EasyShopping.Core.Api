using Application.UseCases.Categories.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCategoryAsync(
            [FromBody] CreateCategoryCommand createCategoryCommand)
        {
            var result = await this.mediator.Send(createCategoryCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }
    }
}

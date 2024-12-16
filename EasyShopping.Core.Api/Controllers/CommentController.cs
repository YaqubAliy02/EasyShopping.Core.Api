using Application.UseCases.Comments.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public CommentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCommentAsync([FromBody] CreateCommentCommand createCommentCommand)
        {
            var result = await this.mediator.Send(createCommentCommand);

            return result.StatusCode is 200 ? Ok(result) : BadRequest(result);
        }
    }
}

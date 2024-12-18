using Application.UseCases.Comments.Command;
using Application.UseCases.Comments.Query;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCommentsAsync()
        {
            return await this.mediator.Send(new GetAllCommentsQuery());
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentByIdCommand updateCommentByIdCommand)
        {
            return await this.mediator.Send(updateCommentByIdCommand);
        }
    }
}

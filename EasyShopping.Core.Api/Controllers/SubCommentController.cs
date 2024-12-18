using Application.UseCases.SubComments.Command;
using Application.UseCases.SubComments.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class SubCommentController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public SubCommentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSubCommentAsync([FromBody] CreateSubCommentCommand createSubCommentCommand)
        {
            var result = await this.mediator.Send(createSubCommentCommand);
            return result.StatusCode is 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllSubCommentAsync()
        {
            return await this.mediator.Send(new GetAllSubCommentQuery());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCommentByIdAsync([FromQuery] GetSubCommentByIdQuery getSubCommentByIdQuery)
        {
            return await this.mediator.Send(getSubCommentByIdQuery);
        }
    }
}

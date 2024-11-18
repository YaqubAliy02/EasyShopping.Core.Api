using Application.UseCases.Users.Command;
using Application.UseCases.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            var result = await this.mediator.Send(createUserCommand);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public Task<IActionResult> GetUserByIdAsync([FromQuery] GetUserByIdQuery getUserByIdQuery)
        {
            return this.mediator.Send(getUserByIdQuery);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return await this.mediator.Send(new GetAllUserQuery());
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserCommand updateUserCommand)
        {
            return await this.mediator.Send(updateUserCommand);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync([FromQuery] DeleteUserCommand deleteUserCommand)
        {
            return await this.mediator.Send(deleteUserCommand);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangeUserPassword changeUserPassword)
        {
            return await this.mediator.Send(changeUserPassword);
        }
    }
}
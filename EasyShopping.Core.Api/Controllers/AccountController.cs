using Application.UseCases.Accounts.Command;
using Application.UseCases.Accounts.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand registerUserCommand)
        {
            var result = await this.mediator.Send(registerUserCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpPost("[action]")]
        public Task<IActionResult> LoginUserAsync(LoginUserCommand loginUserCommand)
        {
            return this.mediator.Send(loginUserCommand);
        }

        [HttpPost]
        [Route("RefreshUserToken")]
        public async Task<IActionResult> RefreshUserToken([FromBody] RefreshTokenCommand refreshTokenCommand)
        {
            var result = await this.mediator.Send(refreshTokenCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] ModifyUserCommand updateUserCommand)
        {
            return  await this.mediator.Send(updateUserCommand);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return  await this.mediator.Send(new GetAllUserQuery());
        }
    }
}

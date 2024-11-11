using Application.UseCases.Accounts.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    }
}

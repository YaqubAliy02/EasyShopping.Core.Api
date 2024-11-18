using Application.UseCases.Roles.Command;
using Application.UseCases.Roles.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : ApiControllerBase
    {
        private IMediator mediator;

        public RoleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand createRoleCommand)
        {
            var result = await this.mediator.Send(createRoleCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoleAsync()
        {
            return await this.mediator.Send(new GetAllRolesQuery());
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoleByIdAsync([FromQuery] GetRoleByIdQuery getRoleByIdQuery)
        {
            return await this.mediator.Send(getRoleByIdQuery);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRoleAsync([FromBody]
        UpdateRoleCommand updateRoleCommand)
        {
            return await this.mediator.Send(updateRoleCommand);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteRoleAsync([FromQuery] DeleteRoleCommand deleteRoleCommand)
        {
            return await this.mediator.Send(deleteRoleCommand);
        }

    }
}

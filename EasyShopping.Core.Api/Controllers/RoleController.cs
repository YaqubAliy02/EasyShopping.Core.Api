using System.Linq.Expressions;
using Application.UseCases.Roles.Command;
using Application.UseCases.Roles.Query;
using Domain.Models;
using MediatR;
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
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand createRoleCommand)
        {
            var result = await this.mediator.Send(createRoleCommand);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetRoleAsync()
        {
            return await this.mediator.Send(new GetAllRolesQuery());
        }
    }
}

using Application.Repository;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Query
{
    public class GetAllRolesQuery : IRequest<IActionResult> { }
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IActionResult>
    {
        private readonly IUserRoleRepository userRoleRepository;

        public GetAllRolesQueryHandler(IUserRoleRepository userRoleRepository)
        {
            this.userRoleRepository = userRoleRepository;
        }

        public async Task<IActionResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<UserRole> role = await this.userRoleRepository.GetAsync(x => true);

            return new OkObjectResult(role);
        }
    }
}

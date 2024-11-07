using Application.Repository;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Query
{
    public class GetRoleByIdQuery : IRequest<IActionResult>
    {
        public Guid RoleId { get; set; }
    }
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, IActionResult>
    {
        private readonly IUserRoleRepository userRoleRepository;

        public GetRoleByIdQueryHandler(IUserRoleRepository userRoleRepository)
        {
            this.userRoleRepository = userRoleRepository;
        }

        public async Task<IActionResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            UserRole userRole = await this.userRoleRepository.GetByIdAsync(request.RoleId);

            if (userRole is null)
                return new NotFoundObjectResult($"User Role Id: {request.RoleId} is not found");

            return new OkObjectResult(userRole);
        }
    }
}

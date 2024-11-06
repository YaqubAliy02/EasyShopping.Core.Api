using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Command
{
    public class UpdateRoleCommand : IRequest<IActionResult>
    {
        public Guid RoleId { get; set; }
        public string Role { get; set; }
    }
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, IActionResult>
    {
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IMapper mapper;

        public UpdateRoleCommandHandler(
            IUserRoleRepository userRoleRepository,
            IMapper mapper)
        {
            this.userRoleRepository = userRoleRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            UserRole role = this.mapper.Map<UserRole>(request);
            role = await this.userRoleRepository.UpdateAsync(role);

            if (role is null)
                return new NotFoundObjectResult("Role is not found to complete update operation!");

            return new OkObjectResult(role);
        }
    }
}

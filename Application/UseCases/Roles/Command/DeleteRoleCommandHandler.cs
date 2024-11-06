using Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Command
{
    public class DeleteRoleCommand : IRequest<IActionResult>
    {
        public Guid RoleId { get; set; }
    }
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, IActionResult>
    {
        private readonly IUserRoleRepository userRoleRepository;

        public DeleteRoleCommandHandler(IUserRoleRepository userRoleRepository)
        {
            this.userRoleRepository = userRoleRepository;
        }

        public async Task<IActionResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await this.userRoleRepository.DeleteAsync(request.RoleId);
            return isDelete ? new OkObjectResult("Role is deleted successfully") :
               new BadRequestObjectResult("Deleted operation failed");
        }
    }
}

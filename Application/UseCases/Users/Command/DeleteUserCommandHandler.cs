using Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Command
{
    public class DeleteUserCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IActionResult>
    {
        private readonly IUserRepository userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await this.userRepository.DeleteAsync(request.Id);

            return isDelete ? new OkObjectResult("User is deleted successfully")
                : new BadRequestObjectResult("Delete operation is failed");
        }
    }
}

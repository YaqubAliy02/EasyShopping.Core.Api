using Application.Extensions;
using Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Command
{
    public class ChangeUserPassword : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
    public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPassword, IActionResult>
    {
        private readonly IUserRepository userRepository;

        public ChangeUserPasswordHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(ChangeUserPassword request, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.GetByIdAsync(request.Id);

            if (user is null)
                return new NotFoundObjectResult("User not found");

            if (user.Password != request.CurrentPassword.GetHash())
                return new BadRequestObjectResult("Current password is incorrect");

            if (request.NewPassword != request.ConfirmPassword)
                return new BadRequestObjectResult("New password and confirmation do not match");

            user.Password = request.NewPassword.GetHash();
            await this.userRepository.UpdatePasswordAsync(user);
            return new OkObjectResult("Password changed successfully ✅");
        }
    }
}

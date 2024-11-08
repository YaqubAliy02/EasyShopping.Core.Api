using Application.Extensions;
using Application.Repository;
using AutoMapper;
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

            if (user is not null)
            {
                string newPassword = request.CurrentPassword.GetHash();
                if (newPassword == user.Password && request.NewPassword == request.ConfirmPassword)
                {
                    user.Password = request.NewPassword.GetHash();
                    await this.userRepository.UpdateAsync(user);

                    return new OkObjectResult("Password is changed successfully✅");
                }
                else new BadRequestObjectResult("Incorrect password");
            }
            return new BadRequestObjectResult("User is not found");
        }
    }
}

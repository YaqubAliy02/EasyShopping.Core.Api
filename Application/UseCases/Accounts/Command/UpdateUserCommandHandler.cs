using System.ComponentModel.DataAnnotations;
using Application.DTOs.Users;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Accounts.Command
{
    public class UpdateUserCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IActionResult>
    {
        public readonly IMapper mapper;
        public readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(
            IMapper mapper, 
            IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = this.mapper.Map<User>(request);
            user = await this.userRepository.UpdateAsync(user);

            if (user is null) 
                return new BadRequestObjectResult(request);

            var userGetDto = this.mapper.Map<UserGetDto>(user);

            return new OkObjectResult(userGetDto);
        }
    }
}

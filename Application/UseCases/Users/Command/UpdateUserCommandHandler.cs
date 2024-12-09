using System.ComponentModel.DataAnnotations;
using Application.DTOs.Users;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Command
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
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = this.mapper.Map<User>(request);
            if (user is null) return new BadRequestObjectResult(request);
            user = await this.userRepository.UpdateAsync(user);
            var userGetDto = this.mapper.Map<UserGetDto>(user);

            return new OkObjectResult(userGetDto);
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Domain.Models;
using Application.DTOs.User;

namespace Application.UseCases.Users.Command
{
    public class UpdateUserCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
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
            var user =  this.mapper.Map<User>(request);
            user = await this.userRepository.UpdateAsync(user);

            if (user is null) return new BadRequestObjectResult(request);

            var userGetDto = this.mapper.Map<UserGetDto>(user);

            return new OkObjectResult(userGetDto);
        }
    }
}

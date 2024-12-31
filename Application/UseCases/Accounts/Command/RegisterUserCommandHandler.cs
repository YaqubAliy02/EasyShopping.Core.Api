using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Abstraction;
using Application.DTOs.Users;
using Application.Models;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Accounts.Command
{
    public class RegisterUserCommand : IRequest<IActionResult>
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IActionResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IValidator<User> validator;
        private readonly IUserRoleRepository roleRepository;
        private readonly ITokenService tokenService;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<User> validator,
            IUserRoleRepository roleRepository,
            ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.validator = validator;
            this.roleRepository = roleRepository;
            this.tokenService = tokenService;
        }

        public async Task<IActionResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            /*var defaultRole = await this.roleRepository.GetRoleByNameAsync("User");
            request.RolesId = new Guid[] { defaultRole.RoleId };*/
            var users = await this.userRepository.GetAsync(x => true);

            foreach (var existUser in users)
            {
                if (request.Email == existUser.Email)
                    return new BadRequestObjectResult("Email is already exist");
            }

            User user = mapper.Map<User>(request);
            var validationResult = validator.Validate(user);

            if (!validationResult.IsValid)
                return new BadRequestObjectResult(validationResult.Errors);

            if (user is null)
                return new BadRequestObjectResult("User is not valid");

            List<UserRole> roles = new List<UserRole>();
            if (user.UserRoles is not null)
            {
                for (int i = 0; i < user.UserRoles.Count; i++)
                {
                    UserRole role = user.UserRoles.ToArray()[i];
                    role = await roleRepository.GetByIdAsync(role.RoleId);

                    if (role is null)
                        return new BadRequestObjectResult($"Role + {role.RoleId} + is not found");

                    roles.Add(role);
                }
            }

            user.UserRoles = roles;
            user.CreatedAt = DateTime.UtcNow;
            user = await this.userRepository.AddAsync(user);

            var userGetDto = this.mapper.Map<UserGetDto>(user);

            if (request is not null)
            {
                UserRegisterDto userRegisterDto = new UserRegisterDto()
                {
                    User = userGetDto,
                    UserToken = await tokenService.CreateTokenAsync(user)
                };

                return new OkObjectResult(userRegisterDto);
            }

            return new BadRequestObjectResult("User is not valid!");
        }
    }
}

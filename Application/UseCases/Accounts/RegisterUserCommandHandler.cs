using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using Application.Models;
using Application.Repository;
using AutoMapper;
using FluentValidation;
using Domain.Models;
using Application.DTOs.Users;
using Application.Abstraction;

namespace Application.UseCases.Accounts
{
    public class RegisterUserCommand : IRequest<ResponseCore<RegisterUserCommandResult>>
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseCore<RegisterUserCommandResult>>
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

        public async Task<ResponseCore<RegisterUserCommandResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<RegisterUserCommandResult>();

            var defaultRole = await this.roleRepository.GetRoleByNameAsync("User");
            request.RolesId = new Guid[] { defaultRole.RoleId };
            User user = this.mapper.Map<User>(request);
            var validationResult = this.validator.Validate(user);

            if (!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if (user is null)
            {
                result.ErrorMessage = new string[] { "User is not found" };
                result.StatusCode = 404;

                return result;
            }
            List<UserRole> roles = new List<UserRole>();
            if (user.UserRoles is not null)
            {
                for (int i = 0; i < user.UserRoles.Count; i++)
                {
                    UserRole role = user.UserRoles.ToArray()[i];
                    role = await this.roleRepository.GetByIdAsync(role.RoleId);

                    if (role is null)
                    {
                        result.ErrorMessage = new string[] { "Role Id: " + role.RoleId + "is not found" };
                        result.StatusCode = 404;

                        return result;
                    }
                    roles.Add(role);
                }
            }
            user.UserRoles = roles;

            if (request is not null)
            {
                UserRegisterDto userRegisterDto = new UserRegisterDto()
                {
                    User = user,
                    UserToken = await this.tokenService.CreateTokenAsync(user)
                };
            }
            user = await this.userRepository.AddAsync(user);

            result.Result = this.mapper.Map<RegisterUserCommandResult>(user);
            result.StatusCode = 200;

            return result;
        }
    }



    public class RegisterUserCommandResult
        {
            public string UserName { get; set; }
            [EmailAddress]
            public string Email { get; set; }
            [PasswordPropertyText]
            public string Password { get; set; }
            public Guid[] RolesId { get; set; }
        }
    }

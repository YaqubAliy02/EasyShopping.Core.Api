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

namespace Application.UseCases.Accounts.Command
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

            /*var defaultRole = await this.roleRepository.GetRoleByNameAsync("User");
            request.RolesId = new Guid[] { defaultRole.RoleId };*/
            var users = await this.userRepository.GetAsync(x => true);

            foreach (var existUser in users)
            {
                if (request.Email == existUser.Email)
                {
                    result.ErrorMessage = new string[] { "Email is already exist" };
                    result.StatusCode = 403;

                    return result;
                }
            }

            User user = mapper.Map<User>(request);
            var validationResult = validator.Validate(user);

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
                    role = await roleRepository.GetByIdAsync(role.RoleId);

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
            user.CreatedAt = DateTime.UtcNow;
            var userGetDto = this.mapper.Map<UserGetDto>(user);

            if (request is not null)
            {
                UserRegisterDto userRegisterDto = new UserRegisterDto()
                {
                    User = userGetDto,
                    UserToken = await tokenService.CreateTokenAsync(user)
                };
            }
            user = await userRepository.AddAsync(user);

            result.Result = mapper.Map<RegisterUserCommandResult>(user);
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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Users.Command
{
    public class CreateUserCommand : IRequest<ResponseCore<CreateUserCommandHandlerResult>>
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseCore<CreateUserCommandHandlerResult>>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IUserRoleRepository roleRepository;
        private readonly IValidator<User> validator;

        public CreateUserCommandHandler(
            IMapper mapper,
            IUserRepository userRepository,
            IUserRoleRepository roleRepository,
            IValidator<User> validator)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.validator = validator;
        }

        public async Task<ResponseCore<CreateUserCommandHandlerResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateUserCommandHandlerResult>();
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
                result.StatusCode = 400;

                return result;
            }
            List<UserRole> roles = new();
            for (int i = 0; i < user.UserRoles.Count; i++)
            {
                UserRole role = user.UserRoles.ToArray()[i];
                role = await this.roleRepository.GetByIdAsync(role.RoleId);

                if (role is null)
                {
                    result.ErrorMessage = new string[] { "Role Id: " + role.RoleId + "Not found" };
                    result.StatusCode = 400;

                    return result;
                }
                roles.Add(role);
            }
            user.UserRoles = roles;

            user = await this.userRepository.AddAsync(user);

            result.Result = this.mapper.Map<CreateUserCommandHandlerResult>(user);
            result.StatusCode = 200;

            return result;
        }
    }
    public class CreateUserCommandHandlerResult
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
}

using Application.Models;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Roles.Command
{
    public class CreateRoleCommand : IRequest<ResponseCore<CreateRoleCommandResult>>
    {
        public string Role { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ResponseCore<CreateRoleCommandResult>>
    {
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IMapper mapper;
        private readonly IValidator<UserRole> validator;

        public CreateRoleCommandHandler(
            IUserRoleRepository userRoleRepository,
            IMapper mapper,
            IValidator<UserRole> validator)
        {
            this.userRoleRepository = userRoleRepository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<ResponseCore<CreateRoleCommandResult>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateRoleCommandResult>();

            UserRole userRole = this.mapper.Map<UserRole>(request);
            var validationResult = this.validator.Validate(userRole);

            if (!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }
            if (userRole is null)
            {
                result.ErrorMessage = new string[] { "Role is not found" };
                result.StatusCode = 400;

                return result;
            }

            userRole = await this.userRoleRepository.AddAsync(userRole);
            result.Result = this.mapper.Map<CreateRoleCommandResult>(userRole);
            result.StatusCode = 200;

            return result;
        }
    }

    public class CreateRoleCommandResult
    {
        public string Role { get; set; }
    }
}

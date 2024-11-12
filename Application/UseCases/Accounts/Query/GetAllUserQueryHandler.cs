using Application.DTOs.Users;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Accounts.Query
{
    public class GetAllUserQuery : IRequest<IActionResult> { }
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public GetAllUserQueryHandler(
            IMapper mapper,
            IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await this.userRepository.GetAsync(x => true);

            var resultUser = this.mapper
                .Map<IEnumerable<UserGetDto>>(users.AsEnumerable());

            return new OkObjectResult(users);
        }
    }
}

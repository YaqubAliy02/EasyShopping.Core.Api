using Application.DTOs.User;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Query
{
    public class GetAllUserQuery : IRequest<IActionResult> { }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUserQuery, IActionResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public GetAllUsersQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
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
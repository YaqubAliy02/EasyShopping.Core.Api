using Application.DTOs.Users;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Query
{
    public class GetUserByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, IActionResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.GetByIdAsync(request.Id);

            if (user is null)
                return new NotFoundObjectResult($"User Id: {request.Id} is not found");

            var userResult = this.mapper.Map<UserGetDto>(user);

            return new OkObjectResult(userResult);
        }
    }
}

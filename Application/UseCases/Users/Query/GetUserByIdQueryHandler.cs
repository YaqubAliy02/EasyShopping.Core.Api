using Application.Repository;
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

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User user = await this.userRepository.GetByIdAsync(request.Id);
            if (user is null)
                return new NotFoundObjectResult($"User Id: {request.Id} is not found");

            return new OkObjectResult(user);
        }
    }
}

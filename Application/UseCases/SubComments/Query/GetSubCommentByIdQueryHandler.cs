using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.SubComments.Query
{
    public class GetSubCommentByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetSubCommentByIdQueryHandler : IRequestHandler<GetSubCommentByIdQuery, IActionResult>
    {
        private readonly ISubCommentRepository subCommentRepository;
        private readonly IMapper mapper;
        public GetSubCommentByIdQueryHandler(ISubCommentRepository subCommentRepository, IMapper mapper)
        {
            this.subCommentRepository = subCommentRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetSubCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var subComment = await this.subCommentRepository.GetByIdAsync(request.Id);

            if (subComment is null)
                return new NotFoundObjectResult("SubComment is not found");

            var result = this.mapper.Map<SubComment>(subComment);

            return new OkObjectResult(result);
        }
    }
}

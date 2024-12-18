using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.SubComments.Query
{
    public class GetAllSubCommentQuery : IRequest<IActionResult> { }

    public class GetAllSubCommentQueryHandler : IRequestHandler<GetAllSubCommentQuery, IActionResult>
    {
        private readonly ISubCommentRepository subCommentRepository;
        private readonly IMapper mapper;

        public GetAllSubCommentQueryHandler(
            ISubCommentRepository subCommentRepository,
            IMapper mapper)
        {
            this.subCommentRepository = subCommentRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllSubCommentQuery request, CancellationToken cancellationToken)
        {
            var subComments = await this.subCommentRepository.GetAsync(x => true);

            var result = this.mapper.Map<IEnumerable<SubComment>>(subComments);

            return new OkObjectResult(result);
        }
    }
}

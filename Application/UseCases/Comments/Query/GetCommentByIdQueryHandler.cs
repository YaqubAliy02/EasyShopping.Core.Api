using Application.DTOs.Comments;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Comments.Query
{
    public class GetCommentByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, IActionResult>
    {
        private readonly ICommentRepository commentRepository;
        private readonly IMapper mapper;

        public GetCommentByIdQueryHandler(
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            this.commentRepository = commentRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
           var comment = await this.commentRepository.GetByIdAsync(request.Id);
           var resultComment = this.mapper.Map<GetCommentDto>(comment);

            if (resultComment is null)
                return new NotFoundObjectResult("Comment is not found!");

            return new OkObjectResult(resultComment);
        }
    }
}

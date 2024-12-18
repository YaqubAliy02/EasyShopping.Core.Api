using Application.DTOs.Comments;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Comments.Query
{
    public class GetAllCommentsQuery : IRequest<IActionResult> { }
    public class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, IActionResult>
    {
        private readonly ICommentRepository commentRepository;
        private readonly IMapper mapper;

        public GetAllCommentsQueryHandler(
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            this.commentRepository = commentRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await this.commentRepository.GetAsync(x => true);

            var result = this.mapper.Map<IEnumerable<GetCommentDto>>(comments);

            return new OkObjectResult(result);
        }
    }
}

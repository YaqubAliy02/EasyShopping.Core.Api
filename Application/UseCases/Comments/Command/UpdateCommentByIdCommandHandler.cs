using Application.DTOs.Comments;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Comments.Command
{
    public class UpdateCommentByIdCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
    public class UpdateCommentByIdCommandHandler : IRequestHandler<UpdateCommentByIdCommand, IActionResult>
    {
        private readonly ICommentRepository commentRepository;
        private readonly IMapper mapper;

        public UpdateCommentByIdCommandHandler(
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            this.commentRepository = commentRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateCommentByIdCommand request, CancellationToken cancellationToken)
        {
            var comment = this.mapper.Map<Comment>(request);
            var update = await this.commentRepository.UpdateAsync(comment);
            var updatedComment = this.mapper.Map<GetCommentDto>(update);

            if (updatedComment is null)
                return new NotFoundObjectResult("Comment is not found to complete update operation!");

            return new OkObjectResult(updatedComment);
        }
    }
}

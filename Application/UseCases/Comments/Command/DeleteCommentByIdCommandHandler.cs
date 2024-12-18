using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Comments.Command
{
    public class DeleteCommentByIdCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteCommentByIdCommandHandler : IRequestHandler<DeleteCommentByIdCommand, IActionResult>
    {
        private readonly ICommentRepository commentRepository;

        public DeleteCommentByIdCommandHandler(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<IActionResult> Handle(DeleteCommentByIdCommand request, CancellationToken cancellationToken)
        {
            bool isDeleted = await this.commentRepository.DeleteAsync(request.Id);

            if (isDeleted)
                return new OkObjectResult("Comment is deleted successfully!");

            return new NotFoundObjectResult("Comment is not found to complete delete operation!");
        }
    }
}

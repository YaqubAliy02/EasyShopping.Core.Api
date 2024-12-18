using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.SubComments.Command
{
    public class DeleteSubCommentCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteSubCommentCommandHandler : IRequestHandler<DeleteSubCommentCommand, IActionResult>
    {
        private readonly ISubCommentRepository subCommentRepository;
        private readonly IMapper mapper;

        public DeleteSubCommentCommandHandler(
            ISubCommentRepository subCommentRepository, 
            IMapper mapper)
        {
            this.subCommentRepository = subCommentRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(DeleteSubCommentCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await this.subCommentRepository.DeleteAsync(request.Id);

            if (isDelete)
                return new OkObjectResult("SubComment is deleted successfully");

            return new NotFoundObjectResult("SubComment is not found to complete operation");
        }
    }
}

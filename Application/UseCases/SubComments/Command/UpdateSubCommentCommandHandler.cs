using Application.DTOs.SubComments;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.SubComments.Command
{
    public class UpdateSubCommentCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
    public class UpdateSubCommentCommandHandler : IRequestHandler<UpdateSubCommentCommand, IActionResult>
    {
        private readonly ISubCommentRepository subCommentRepository;
        private readonly IMapper mapper;

        public UpdateSubCommentCommandHandler(ISubCommentRepository subCommentRepository, IMapper mapper)
        {
            this.subCommentRepository = subCommentRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateSubCommentCommand request, CancellationToken cancellationToken)
        {
            var subComment = this.mapper.Map<SubComment>(request);
            var updateSubComment = await this.subCommentRepository.UpdateAsync(subComment);
            
            var updatedSubComment = this.mapper.Map<GetSubCommentDto>(updateSubComment);

            if (updateSubComment is null)
                return new NotFoundObjectResult("SubComment is not found to complete operation");

            return new OkObjectResult(updatedSubComment);
        }
    }
}

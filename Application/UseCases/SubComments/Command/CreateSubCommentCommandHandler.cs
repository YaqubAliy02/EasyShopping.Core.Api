using Application.Models;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.UseCases.SubComments.Command
{
    public class CreateSubCommentCommand : IRequest<ResponseCore<CreateSubCommentCommandHandlerResult>>
    {
        public string Text { get; set; }
        public Guid CommentId { get; set; }
    }
    public class CreateSubCommentCommandHandler : IRequestHandler<CreateSubCommentCommand, ResponseCore<CreateSubCommentCommandHandlerResult>>
    {
        private readonly ISubCommentRepository subCommentRepository;
        private readonly IMapper mapper;
        private readonly IValidator<SubComment> validator;

        public CreateSubCommentCommandHandler(
            ISubCommentRepository subCommentRepository,
            IMapper mapper,
            IValidator<SubComment> validator)
        {
            this.subCommentRepository = subCommentRepository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<ResponseCore<CreateSubCommentCommandHandlerResult>> Handle(CreateSubCommentCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateSubCommentCommandHandlerResult>();

            var subComment = this.mapper.Map<SubComment>(request);
            var validationResult = this.validator.Validate(subComment);

            if(!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if(subComment is null)
            {
                result.ErrorMessage = new string[] { "SubComment is not found" };
                result.StatusCode = 404;

                return result;
            }

            subComment =  await this.subCommentRepository.AddAsync(subComment);
            result.Result = this.mapper.Map<CreateSubCommentCommandHandlerResult>(subComment);
            result.StatusCode = 200;

            return result;
        }
    }
    public class CreateSubCommentCommandHandlerResult
    {
        public string Text { get; set; }
        public Guid CommentId { get; set; }
    }
}

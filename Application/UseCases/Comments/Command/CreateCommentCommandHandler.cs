using Application.Models;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Comments.Command
{
    public class CreateCommentCommand : IRequest<ResponseCore<CreateCommentCommandHandlerResult>>
    {
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ResponseCore<CreateCommentCommandHandlerResult>>
    {
        private readonly IMapper mapper;
        private readonly ICommentRepository commentRepository;
        private readonly IValidator<Comment> validator;

        public CreateCommentCommandHandler(
            IMapper mapper,
            ICommentRepository commentRepository,
            IValidator<Comment> validator)
        {
            this.mapper = mapper;
            this.commentRepository = commentRepository;
            this.validator = validator;
        }

        public async Task<ResponseCore<CreateCommentCommandHandlerResult>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateCommentCommandHandlerResult>();

            var comment = this.mapper.Map<Comment>(request);
            var validationResult = this.validator.Validate(comment);

            if (!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }
            if (comment is null)
            {
                result.ErrorMessage = new string[] { "Comment is not found" };
                result.StatusCode = 404;

                return result;
            }

            comment = await this.commentRepository.AddAsync(comment);

            result.Result = this.mapper.Map<CreateCommentCommandHandlerResult>(comment);
            result.StatusCode = 200;

            return result;

        }
    }

    public class CreateCommentCommandHandlerResult
    {
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}

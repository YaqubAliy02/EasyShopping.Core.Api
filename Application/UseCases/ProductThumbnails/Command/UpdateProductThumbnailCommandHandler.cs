using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.ProductThumbnails.Command
{
    public class UpdateProductThumbnailCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public IFormFile IFormFile { get; set; }
    }
    public class UpdateProductThumbnailCommandHandler : IRequestHandler<UpdateProductThumbnailCommand, IActionResult>
    {
        private readonly IProductThumbnailRepository productThumbnailRepository;
        private readonly IMapper mapper;
        public UpdateProductThumbnailCommandHandler(
            IProductThumbnailRepository productThumbnailRepository,
            IMapper mapper)
        {
            this.productThumbnailRepository = productThumbnailRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateProductThumbnailCommand request, CancellationToken cancellationToken)
        {
            var existingProductThumbnail = await this.productThumbnailRepository.GetByIdAsync(request.Id);
            if (existingProductThumbnail is null)
                return new NotFoundObjectResult("ProductThumbnail is not found:(");

            if (request.IFormFile is null || !ValidateProductThumbnail(request.IFormFile))
            {
                return new BadRequestObjectResult("Thumbnail is not valid format");
            }

            using var stream = request.IFormFile.OpenReadStream();
            var blobResult = await this.productThumbnailRepository
                .AddForUpdateProductThumbnailAsync(stream, request.IFormFile.FileName, request.IFormFile.ContentType);

            blobResult.ProductId = existingProductThumbnail.Id;

            var updatedProductThumbnail = this.productThumbnailRepository.UpdateAsync(blobResult);

            if (updatedProductThumbnail is null)
                return new NotFoundObjectResult("ProductThumbnail is not found to complete update operation!");

            return new OkObjectResult(updatedProductThumbnail);

        }

        private bool ValidateProductThumbnail(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return file.Length > 0 && file.Length < 50 * 1024 * 1024 && allowedExtensions.Contains(extension);
        }
    }
}

using Application.Repository;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.ProductThumbnails.Command
{
    public class UploadProductThumbnailCommand : IRequest<IActionResult>
    {
        public IFormFile IFormFile { get; set; }
        public Guid ProductId { get; set; }
    }
    public class UploadProductThumbnailCommandHandler : IRequestHandler<UploadProductThumbnailCommand, IActionResult>
    {
        private readonly IProductThumbnailRepository productThumbnailRepository;

        public UploadProductThumbnailCommandHandler(
            IProductThumbnailRepository productThumbnailRepository)
        {
            this.productThumbnailRepository = productThumbnailRepository;
        }

        public async Task<IActionResult> Handle(UploadProductThumbnailCommand request, CancellationToken cancellationToken)
        {
            if (request.IFormFile is null || !ValidateProductThumbnail(request.IFormFile))
            {
                return new BadRequestObjectResult("Thumbnail is not valid format");
            }

            using var stream = request.IFormFile.OpenReadStream();
            var blobResult = await this.productThumbnailRepository.AddProductThumbnailAsync(request.ProductId, stream, request.IFormFile.FileName, request.IFormFile.ContentType);

            var productThumbnail = new ProductThumbnail
            {
                Id = Guid.NewGuid(),
                FileName = request.IFormFile.FileName,
                ContentType = request.IFormFile.ContentType,
                Size = request.IFormFile.Length,
                BlobUri = blobResult.BlobUri,
                UploadedDate = DateTime.UtcNow
            };

            return new OkObjectResult(productThumbnail);
        }

        private bool ValidateProductThumbnail(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return file.Length > 0 && file.Length < 50 * 1024 * 1024 && allowedExtensions.Contains(extension);
        }
    }
}

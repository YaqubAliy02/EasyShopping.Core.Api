using Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Products.Command
{
    public class DeleteProductCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, IActionResult>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await this.productRepository.DeleteAsync(request.Id);

            if (isDelete)
                return new OkObjectResult("Product is deleted successfully");

            return new BadRequestObjectResult("Delete operation has been failed");
        }
    }
}

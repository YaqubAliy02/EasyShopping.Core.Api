using Application.Abstraction;
using Application.Models;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Products.Command
{
    public class CreateProductCommand : IRequest<ResponseCore<CreateProductCommandHandlerResult>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public string Thumbnail { get; set; }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseCore<CreateProductCommandHandlerResult>>
    {
        private readonly IMapper mapper;
        private readonly IValidator<Product> validator;
        private readonly IProductRepository productRepository;
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public CreateProductCommandHandler(
            IMapper mapper,
            IValidator<Product> validator,
            IProductRepository productRepository,
            IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.mapper = mapper;
            this.validator = validator;
            this.productRepository = productRepository;
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<ResponseCore<CreateProductCommandHandlerResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateProductCommandHandlerResult>();

            Product product = this.mapper.Map<Product>(request);
            var validationResult = this.validator.Validate(product);

            if(!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if(product is null)
            {
                result.ErrorMessage = new string[] { "Product is not found" };
                result.StatusCode = 404;

                return result;
            }

            product = await this.productRepository.AddAsync(product);

            result.Result = this.mapper.Map<CreateProductCommandHandlerResult>(product);
            result.StatusCode = 200;

            return result;

        }
    }

    public class CreateProductCommandHandlerResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public string Thumbnail { get; set; }
    }
}

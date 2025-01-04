using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;
using Infrastracture.External.AWSS3;
using Infrastracture.External.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

namespace Infrastracture.Services
{
    public class ProductThumbnailRepository : IProductThumbnailRepository
    {
        private readonly IAWSStorage awsStorage;
        private readonly IProductRepository productRepository;
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public ProductThumbnailRepository(
            IAWSStorage awsStorage,
            IProductRepository productRepository,
            IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.awsStorage = awsStorage;
            this.productRepository = productRepository;
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<ProductThumbnail> AddProductThumbnailAsync(Guid productId, Stream photoStream, string fileName, string contentType)
        {
            var product = await this.productRepository.GetByIdAsync(productId);

            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            string s3Uri = await this.awsStorage.UploadFileAsync(photoStream, fileName, contentType);

            var productThumbnail = new ProductThumbnail
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                BlobUri = s3Uri,
                FileName = Path.GetFileName(fileName),
                Size = photoStream.Length,
                ContentType = contentType,
                UploadedDate = DateTimeOffset.UtcNow
            };

            await AddAsync(productThumbnail);

            return productThumbnail;
        }

        public async Task<ProductThumbnail> AddForUpdateProductThumbnailAsync(Stream photoStream, string fileName, string contentType)
        {
            string s3Uri = await this.awsStorage.UploadFileAsync(photoStream, fileName, contentType);

            var productThumbnail = new ProductThumbnail
            {
                Id = Guid.NewGuid(),
                BlobUri = s3Uri,
                FileName = Path.GetFileName(fileName),
                Size = photoStream.Length,
                ContentType = contentType,
                UploadedDate = DateTimeOffset.UtcNow
            };

            await AddAsync(productThumbnail);

            return productThumbnail;
        }

        public async Task<ProductThumbnail> AddAsync(ProductThumbnail productThumbnail)
        {
            await this.easyShoppingDbContext.ProductThumbnails.AddAsync(productThumbnail);
            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            return result > 0 ? productThumbnail : null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var productThumbnail = await this.easyShoppingDbContext.ProductThumbnails.FindAsync(id);

            if (productThumbnail is not null)
            {
                await this.awsStorage.DeleteFileAsync(productThumbnail.FileName);

                this.easyShoppingDbContext.ProductThumbnails.Remove(productThumbnail);
            }

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IQueryable<ProductThumbnail>> GetAsync(Expression<Func<ProductThumbnail, bool>> expression)
        {
            return this.easyShoppingDbContext.ProductThumbnails
                .Where(expression)
                .AsQueryable();
        }

        public async Task<ProductThumbnail> GetByIdAsync(Guid id)
        {
            return await this.easyShoppingDbContext.ProductThumbnails
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ProductThumbnail> UpdateAsync(ProductThumbnail updateProductThumbnail)
        {
            var existingProductThumbnail = await this.GetByIdAsync(updateProductThumbnail.Id);

            if (existingProductThumbnail is not null)
            {
                this.easyShoppingDbContext.ProductThumbnails.Update(updateProductThumbnail);
                int result = await this.easyShoppingDbContext.SaveChangesAsync();

                if (result > 0) return updateProductThumbnail;
            }

            return null;
        }
    }
}

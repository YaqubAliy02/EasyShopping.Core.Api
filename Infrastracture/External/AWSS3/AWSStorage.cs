using Amazon.S3.Model;
using Amazon.S3;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Amazon;

namespace Infrastracture.External.AWSS3
{
    public class AWSStorage : IAWSStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        private readonly HashSet<string> supportedPhotoExtensions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif"
            };

        private readonly HashSet<string> supportedPhotoContentTypes =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "image/jpeg",
                "image/png",
                "image/gif"
            };

        public AWSStorage(IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection("AWS");
            _bucketName = awsOptions["BucketName"];

            _s3Client = new AmazonS3Client(
                awsOptions["AccessKey"],
                awsOptions["SecretKey"],
                RegionEndpoint.GetBySystemName(awsOptions["Region"])
            );
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            if (!supportedPhotoExtensions.Contains(extension) || !supportedPhotoContentTypes.Contains(contentType))
            {
                throw new InvalidOperationException("Unsupported file extension or content type.");
            }

            var uploadRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,
            };

            await _s3Client.PutObjectAsync(uploadRequest);

            return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }

        public async Task<List<ProductThumbnail>> GetAllFilesAsync()
        {
            var photos = new List<ProductThumbnail>();

            var listRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName
            };

            var listResponse = await _s3Client.ListObjectsV2Async(listRequest);

            foreach (var s3Object in listResponse.S3Objects)
            {
                var extension = Path.GetExtension(s3Object.Key).ToLower();
                if (supportedPhotoExtensions.Contains(extension))
                {
                    var metadataRequest = new GetObjectMetadataRequest
                    {
                        BucketName = _bucketName,
                        Key = s3Object.Key
                    };

                    var metadataResponse = await _s3Client.GetObjectMetadataAsync(metadataRequest);

                    photos.Add(new ProductThumbnail
                    {
                        Id = Guid.NewGuid(),
                        FileName = s3Object.Key,
                        ContentType = metadataResponse.Headers.ContentType,
                        Size = metadataResponse.Headers.ContentLength,
                        BlobUri = $"https://{_bucketName}.s3.amazonaws.com/{s3Object.Key}",
                        UploadedDate = metadataResponse.LastModified.Date,
                    });
                }
            }

            return photos;
        }
    }
}


using Application.Abstractions.Services;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Services
{
    public class MinIOStorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private string _bucketName;

        public MinIOStorageService(IOptionsMonitor<MinIoOptions> optionsMonitor) 
        {
            var minioOptions = optionsMonitor.CurrentValue;
            
            _bucketName = minioOptions.BucketName;

            _minioClient = new MinioClient()
                .WithEndpoint(minioOptions.Endpoint)
                .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                .Build();
        }

        public async Task<string> GetUrlAsync(string objectName)
        {
            var presignedUrlArgs = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithExpiry(60 * 60 * 24);

            var url = await _minioClient.PresignedGetObjectAsync(presignedUrlArgs);
            return url;
        }

        public async Task PutAsync(string name, Stream mediaStream, string contentType)
        {
            var found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(name)
                .WithContentType(contentType)
                .WithObjectSize(mediaStream.Length)
                .WithStreamData(mediaStream);

            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }
    }
}

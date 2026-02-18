using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Sapico.ImageResizer.Plugin.S3Cache
{
    public class S3ImageCache : IImageCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _prefix;

        public S3ImageCache(IMemoryCache memoryCache, IAmazonS3 s3Client, IOptions<S3CacheOptions> options)
        {
            _memoryCache = memoryCache;
            _s3Client = s3Client;
            _bucketName = options.Value.BucketName;
            _prefix = options.Value.Prefix ?? string.Empty;
        }

        public bool TryGet(long key, out byte[] imageBytes)
        {
            if (_memoryCache.TryGetValue(key, out imageBytes))
                return true;

            try
            {
                var response = _s3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = GetS3Key(key)
                }).GetAwaiter().GetResult();

                using var ms = new MemoryStream();
                response.ResponseStream.CopyTo(ms);
                imageBytes = ms.ToArray();
                _memoryCache.Set(key, imageBytes);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                imageBytes = null;
                return false;
            }
        }

        public void Set(long key, byte[] imageBytes)
        {
            _memoryCache.Set(key, imageBytes);

            try
            {
                using var ms = new MemoryStream(imageBytes);
                _s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = GetS3Key(key),
                    InputStream = ms,
                    ContentType = "application/octet-stream"
                }).GetAwaiter().GetResult();
            }
            catch (AmazonS3Exception)
            {
                // Log but don't fail the request if S3 write fails;
                // the in-memory cache still holds the data.
            }
        }

        private string GetS3Key(long key)
        {
            var hex = key.ToString("x16");
            return _prefix + hex.Substring(0, 2) + "/" + hex + ".bin";
        }
    }
}

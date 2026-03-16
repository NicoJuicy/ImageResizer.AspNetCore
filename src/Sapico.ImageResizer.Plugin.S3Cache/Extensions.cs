using System;
using Amazon;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace Sapico.ImageResizer.Plugin.S3Cache
{
    public static class Extensions
    {
        public static IServiceCollection AddImageResizerS3Cache(this IServiceCollection services, Action<S3CacheOptions> configure)
        {
            services.Configure(configure);
            services.AddMemoryCache();

            services.AddSingleton<IAmazonS3>(sp =>
            {
                var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<S3CacheOptions>>().Value;

                if (!string.IsNullOrEmpty(options.AccessKey) && !string.IsNullOrEmpty(options.SecretKey))
                {
                    return new AmazonS3Client(
                        options.AccessKey,
                        options.SecretKey,
                        RegionEndpoint.GetBySystemName(options.Region));
                }

                // Falls back to default credential chain (env vars, IAM role, etc.)
                return new AmazonS3Client(RegionEndpoint.GetBySystemName(options.Region));
            });

            services.AddSingleton<IImageCache, S3ImageCache>();
            return services;
        }
    }
}

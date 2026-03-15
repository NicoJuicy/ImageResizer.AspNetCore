using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sapico.ImageResizer.Plugin.DiskCache
{
    public static class Extensions
    {
        public static IServiceCollection AddImageResizerDiskCache(this IServiceCollection services, Action<DiskCacheOptions>? configure = null)
        {
            if (configure != null)
                services.Configure(configure);
            else
                services.Configure<DiskCacheOptions>(_ => { });

            services.AddSingleton<IImageCache, DiskImageCache>();
            return services;
        }
    }
}

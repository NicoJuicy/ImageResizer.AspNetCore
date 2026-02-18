using System;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Sapico.ImageResizer.Plugin.DiskCache
{
    public class DiskImageCache : IImageCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheFolder;

        public DiskImageCache(IMemoryCache memoryCache, IOptions<DiskCacheOptions> options)
        {
            _memoryCache = memoryCache;
            _cacheFolder = options.Value.CacheFolder;
            Directory.CreateDirectory(_cacheFolder);
        }

        public bool TryGet(long key, out byte[] imageBytes)
        {
            if (_memoryCache.TryGetValue(key, out imageBytes))
                return true;

            var path = GetPath(key);
            if (File.Exists(path))
            {
                imageBytes = File.ReadAllBytes(path);
                _memoryCache.Set(key, imageBytes);
                return true;
            }

            imageBytes = null;
            return false;
        }

        public void Set(long key, byte[] imageBytes)
        {
            _memoryCache.Set(key, imageBytes);

            var path = GetPath(key);
            var dir = Path.GetDirectoryName(path);
            if (dir != null)
                Directory.CreateDirectory(dir);
            File.WriteAllBytes(path, imageBytes);
        }

        private string GetPath(long key)
        {
            var hex = key.ToString("x16");
            return Path.Combine(_cacheFolder, hex.Substring(0, 2), hex + ".bin");
        }
    }
}

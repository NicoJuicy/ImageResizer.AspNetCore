using System;
using System.IO;
using Microsoft.Extensions.Options;

namespace Sapico.ImageResizer.Plugin.DiskCache
{
    public class DiskImageCache : IImageCache
    {
        private readonly string _cacheFolder;

        public DiskImageCache(IOptions<DiskCacheOptions> options)
        {
            _cacheFolder = options.Value.CacheFolder;
            Directory.CreateDirectory(_cacheFolder);
        }

        public bool TryGet(long key, out byte[] imageBytes)
        {
            var path = GetPath(key);
            if (File.Exists(path))
            {
                imageBytes = File.ReadAllBytes(path);
                return true;
            }

            imageBytes = null;
            return false;
        }

        public void Set(long key, byte[] imageBytes)
        {
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

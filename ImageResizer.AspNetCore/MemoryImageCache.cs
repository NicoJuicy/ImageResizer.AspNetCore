using Microsoft.Extensions.Caching.Memory;

namespace Sapico.ImageResizer
{
    internal class MemoryImageCache : IImageCache
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryImageCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool TryGet(long key, out byte[] imageBytes)
        {
            return _memoryCache.TryGetValue(key, out imageBytes);
        }

        public void Set(long key, byte[] imageBytes)
        {
            _memoryCache.Set(key, imageBytes);
        }
    }
}

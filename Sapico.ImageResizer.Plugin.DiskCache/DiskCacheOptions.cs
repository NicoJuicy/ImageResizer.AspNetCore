using System;
using System.IO;

namespace Sapico.ImageResizer.Plugin.DiskCache
{
    public class DiskCacheOptions
    {
        public string CacheFolder { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "cache");
    }
}

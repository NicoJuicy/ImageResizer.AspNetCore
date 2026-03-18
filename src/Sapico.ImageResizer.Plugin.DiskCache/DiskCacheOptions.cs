using System.IO;

namespace Sapico.ImageResizer.Plugin.DiskCache;

public class DiskCacheOptions
{
    public string CacheFolder { get; set; } = Path.Combine(Path.GetTempPath(), "img-cache"); // Default folder for docker containers
        //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Te), "img-cache");
}

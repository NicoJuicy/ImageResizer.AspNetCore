namespace Sapico.ImageResizer.Plugin.S3Cache
{
    public class S3CacheOptions
    {
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = "us-east-1";
        public string Prefix { get; set; } = string.Empty;
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
    }
}

using System;

namespace Sapico.ImageResizer
{
    public class ImageResizerOptions
    {
        /// <summary>
        /// The max-age value for the Cache-Control header. Defaults to 30 days.
        /// Set to <see cref="TimeSpan.Zero"/> to disable Cache-Control.
        /// </summary>
        public TimeSpan CacheMaxAge { get; set; } = TimeSpan.FromDays(30);

        /// <summary>
        /// Whether to emit an ETag header and honour If-None-Match conditional requests.
        /// </summary>
        public bool EnableETag { get; set; } = true;

        /// <summary>
        /// Whether to emit a Last-Modified header and honour If-Modified-Since conditional requests.
        /// </summary>
        public bool EnableLastModified { get; set; } = true;
    }
}

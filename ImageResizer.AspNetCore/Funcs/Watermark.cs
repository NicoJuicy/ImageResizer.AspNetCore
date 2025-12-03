using ImageResizer.AspNetCore.Helpers;
using ImageResizer.AspNetCore.Models;
using SkiaSharp;
using System;
using System.IO;

namespace ImageResizer.AspNetCore.Funcs
{
    internal static class Watermark
    {
        internal static SKBitmap WatermarkText(SKBitmap original, ResizeParams resizeParams, WatermarkTextModel watermarkText)
        {
            var toBitmap = new SKBitmap(original.Width, original.Height);
            var canvas = new SKCanvas(toBitmap);
            // Draw a bitmap rescaled

            var paint = new SKPaint();
            paint.Style = watermarkText.Type.GetSKPaintStyle();
            paint.IsAntialias = true;

            //https://www.color-hex.com/
            if (SKColor.TryParse(watermarkText.Color, out SKColor color))
            {
                paint.Color = color;
            }
            else
            {
                paint.Color = SKColors.Black;
            }

            canvas.DrawBitmap(original, 0, 0);

            var x = watermarkText.PositionMeasureType == 1 ? watermarkText.X : watermarkText.X.ToPixel(original.Width);
            var y = watermarkText.PositionMeasureType == 1 ? watermarkText.Y : watermarkText.Y.ToPixel(original.Height);

            // Simplified text drawing for SkiaSharp 3.x compatibility
            var font = new SKFont(SKTypeface.Default, watermarkText.TextSize);
            canvas.DrawText(watermarkText.Value, x, y, font, paint);

            canvas.Flush();

            canvas.Dispose();
            paint.Dispose();
            font.Dispose();
            original.Dispose();

            return toBitmap;
        }
        internal static SKBitmap WatermarkImage(SKBitmap original, ResizeParams resizeParams, WatermarkImageModel watermarkImage, string sourceImagePath)
        {
            if (watermarkImage == null || string.IsNullOrWhiteSpace(watermarkImage.Url))
                return original;

            string url = watermarkImage.Url.Trim();
            SKBitmap wmBitmap = null;

            // Handle data URI (base64) if provided
            if (url.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                var comma = url.IndexOf(',');
                if (comma >= 0)
                {
                    var base64 = url.Substring(comma + 1);
                    try
                    {
                        var bytes = System.Convert.FromBase64String(base64);
                        wmBitmap = SKBitmap.Decode(bytes);
                    }
                    catch { /* fall through */ }
                }
            }

            // Try file system locations when not data-uri or decode failed
            if (wmBitmap == null)
            {
                string resolvedPath = null;

                // Absolute path
                if (File.Exists(url))
                    resolvedPath = url;

                // Path relative to source image folder
                if (resolvedPath == null && !string.IsNullOrEmpty(sourceImagePath))
                {
                    var folder = Path.GetDirectoryName(sourceImagePath);
                    var tryPath = Path.Combine(folder ?? string.Empty, url.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(tryPath)) resolvedPath = tryPath;

                    // search for wwwroot in the source path and resolve relative to it
                    if (resolvedPath == null)
                    {
                        var current = new DirectoryInfo(folder ?? string.Empty);
                        while (current != null && !current.Name.Equals("wwwroot", StringComparison.OrdinalIgnoreCase))
                            current = current.Parent;

                        if (current != null)
                        {
                            var try2 = Path.Combine(current.FullName, url.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar));
                            if (File.Exists(try2)) resolvedPath = try2;
                        }
                    }
                }

                if (resolvedPath == null)
                {
                    // final fallback: path relative to working dir
                    var tryLocal = url.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
                    if (File.Exists(tryLocal)) resolvedPath = tryLocal;
                }

                if (resolvedPath != null)
                {
                    try
                    {
                        using var s = File.OpenRead(resolvedPath);
                        wmBitmap = SKBitmap.Decode(s);
                    }
                    catch { wmBitmap = null; }
                }
            }

            if (wmBitmap == null)
                return original; // nothing to do

            // Determine target size
            bool sizeAbsolute = watermarkImage.SizeMeasureType == 1;
            float targetW = watermarkImage.Width;
            float targetH = watermarkImage.Height;

            if (!sizeAbsolute)
            {
                if (targetW > 0) targetW = original.Width * (targetW / 100f);
                if (targetH > 0) targetH = original.Height * (targetH / 100f);
            }

            if (targetW <= 0 && targetH > 0)
                targetW = (wmBitmap.Width * targetH) / wmBitmap.Height;
            else if (targetH <= 0 && targetW > 0)
                targetH = (wmBitmap.Height * targetW) / wmBitmap.Width;
            else if (targetW <= 0 && targetH <= 0)
            {
                // default watermark size: 20% of shortest side
                var defaultSize = Math.Min(original.Width, original.Height) * 0.2f;
                if (wmBitmap.Width >= wmBitmap.Height)
                {
                    targetW = defaultSize;
                    targetH = wmBitmap.Height * (defaultSize / wmBitmap.Width);
                }
                else
                {
                    targetH = defaultSize;
                    targetW = wmBitmap.Width * (defaultSize / wmBitmap.Height);
                }
            }

            var targetInfo = new SKImageInfo(Math.Max(1, (int)Math.Round(targetW)), Math.Max(1, (int)Math.Round(targetH)), wmBitmap.Info.ColorType, wmBitmap.AlphaType);
            var scaled = wmBitmap.Resize(targetInfo, new SKSamplingOptions(SKFilterMode.Linear));
            if (scaled == null)
            {
                // fallback to original watermark
                scaled = wmBitmap.Copy();
            }

            // Determine position
            bool posAbsolute = watermarkImage.PositionMeasureType == 1;
            float left = watermarkImage.Left;
            float top = watermarkImage.Top;

            if (!posAbsolute)
            {
                left = left.ToPixel(original.Width);
                top = top.ToPixel(original.Height);
            }

            if (watermarkImage.Right > 0)
            {
                float right = watermarkImage.Right;
                if (!posAbsolute) right = right.ToPixel(original.Width);
                left = original.Width - right - scaled.Width;
            }
            if (watermarkImage.Bottom > 0)
            {
                float bottom = watermarkImage.Bottom;
                if (!posAbsolute) bottom = bottom.ToPixel(original.Height);
                top = original.Height - bottom - scaled.Height;
            }

            var toBitmap = new SKBitmap(original.Width, original.Height, original.AlphaType == SKAlphaType.Opaque);
            var canvas = new SKCanvas(toBitmap);
            canvas.DrawBitmap(original, 0, 0);

            var paint = new SKPaint { IsAntialias = true };
            // Use watermark alpha if original watermark has alpha; otherwise apply subtle alpha
            if (scaled.AlphaType == SKAlphaType.Opaque)
                paint.Color = new SKColor(255, 255, 255, (byte)(0.7f * 255));

            canvas.DrawBitmap(scaled, (int)Math.Round(left), (int)Math.Round(top), paint);
            canvas.Flush();

            // cleanup
            canvas.Dispose();
            paint.Dispose();
            wmBitmap.Dispose();
            scaled.Dispose();
            original.Dispose();

            return toBitmap;
        }


    }
}

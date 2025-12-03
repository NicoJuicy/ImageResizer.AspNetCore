using ImageResizer.AspNetCore.Helpers;
using ImageResizer.AspNetCore.Models;
using SkiaSharp;

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
            paint.FilterQuality = watermarkText.Quality.GetSKFilterQuality();
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
        internal static SKBitmap WatermarkImage(SKBitmap original, ResizeParams resizeParams, WatermarkImageModel watermarkImage)
        {
            return null;
        }


    }
}

# Sapico.ImageResizer

A lightweight, fast image processing middleware for ASP.NET Core that enables on-the-fly image resizing, cropping, format conversion, and watermarking through URL query parameters.

**Fork of** [ImageResizer.AspNetCore](https://github.com/cornelha/ImageResizer.AspNetCore) by Cornel Hattingh, which itself was forked from [keyone2693/ImageResizer.AspNetCore](https://github.com/keyone2693/ImageResizer.AspNetCore).

## Features

- **Resize images** - Scale images to specific dimensions
- **Crop images** - Extract specific regions from images
- **Format conversion** - Convert between PNG, JPG, and JPEG
- **Image quality control** - Adjust compression quality
- **Auto-rotation** - Automatically rotate images based on EXIF orientation
- **Padding mode** - Add padding to maintain aspect ratio
- **Max mode** - Scale to fit within bounds while maintaining aspect ratio
- **Crop mode** - Crop to exact dimensions
- **Stretch mode** - Stretch image to exact dimensions
- **Watermarking** - Add text or image watermarks
- **Memory caching** - Automatically cache processed images
- **Plugin architecture** - DiskCache and S3Cache plugins for persistent caching
- **Multi-framework support** - Supports .NET 8.0, 9.0, 10.0

## Installation

Install via NuGet:

```bash
dotnet add package Sapico.ImageResizer
```

Optional cache plugins:

```bash
dotnet add package Sapico.ImageResizer.Plugin.DiskCache
dotnet add package Sapico.ImageResizer.Plugin.S3Cache
```

## Quick Start

### 1. Register the Service

In your `Program.cs` (ASP.NET Core 6+):

```csharp
using Sapico.ImageResizer.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add ImageResizer service
builder.Services.AddImageResizer();

var app = builder.Build();

// Use ImageResizer middleware
app.UseStaticFiles();
app.UseImageResizer();

app.Run();
```

### 2. Use in HTML

Simply add query parameters to your image URLs:

```html
<img src="~/images/photo.jpg?w=200" alt="Resized" />
<img src="~/images/photo.jpg?width=200&height=300" alt="Resized with height" />
```

## Usage Examples

### Basic Resizing

Resize to a specific width (height auto-calculated to maintain aspect ratio):

```html
<img src="~/images/photo.jpg?w=300" alt="Width 300px" />
<img src="~/images/photo.jpg?width=300" alt="Width 300px (alternative)" />
```

Resize to a specific height (width auto-calculated):

```html
<img src="~/images/photo.jpg?h=400" alt="Height 400px" />
<img src="~/images/photo.jpg?height=400" alt="Height 400px (alternative)" />
```

### Format Conversion

```html
<img src="~/images/photo.jpg?format=png" alt="PNG format" />
<img src="~/images/photo.png?format=jpg&quality=80" alt="JPEG format" />
```

### Resize Modes

**Max mode** (default) - Fit within bounds maintaining aspect ratio:

```html
<img src="~/images/photo.jpg?w=200&h=200&mode=max" alt="Max fit" />
```

**Pad mode** - Fit within bounds and pad with white space:

```html
<img src="~/images/photo.jpg?w=200&h=200&mode=pad" alt="Padded" />
```

**Crop mode** - Crop to exact dimensions from center:

```html
<img src="~/images/photo.jpg?w=200&h=200&mode=crop" alt="Cropped" />
```

**Stretch mode** - Stretch to exact dimensions (may distort):

```html
<img src="~/images/photo.jpg?w=200&h=200&mode=stretch" alt="Stretched" />
```

### Quality Control

```html
<img src="~/images/photo.jpg?w=800&quality=75" alt="Optimized" />
```

### Auto-Rotation

```html
<img src="~/images/photo.jpg?autorotate=true" alt="Auto-rotated" />
```

### Watermarking

Add text watermark (requires ImageResizerJson.json configuration):

```html
<img src="~/images/photo.jpg?w=400&wmtext=1" alt="With watermark" />
```

Add image watermark:

```html
<img src="~/images/photo.jpg?w=400&wmimage=1" alt="With image watermark" />
```

## Query Parameters Reference

| Parameter | Alias | Type | Default | Description |
|-----------|-------|------|---------|-------------|
| `w` | `width` | int | 0 | Width in pixels (0 = auto-calculate) |
| `h` | `height` | int | 0 | Height in pixels (0 = auto-calculate) |
| `mode` | | string | max | Resize mode: `max`, `pad`, `crop`, `stretch` |
| `format` | | string | original | Output format: `jpg`, `jpeg`, `png` |
| `quality` | | int | 100 | JPEG quality (1-100) |
| `autorotate` | | bool | false | Auto-rotate based on EXIF orientation |
| `wmtext` | | int | 0 | Text watermark ID (requires config) |
| `wmimage` | | int | 0 | Image watermark ID (requires config) |

## Cache Plugins

### DiskCache

Stores processed images on the local filesystem. Default cache folder: `~/cache`.

```csharp
using Sapico.ImageResizer.Plugin.DiskCache;

builder.Services.AddImageResizer();
builder.Services.AddImageResizerDiskCache(options =>
{
    options.CacheFolder = "/var/cache/imageresizer"; // optional, default: ~/cache
});
```

### S3Cache

Stores processed images in an Amazon S3 bucket.

```csharp
using Sapico.ImageResizer.Plugin.S3Cache;

builder.Services.AddImageResizer();
builder.Services.AddImageResizerS3Cache(options =>
{
    options.BucketName = "my-image-cache";
    options.Region = "us-east-1";
    options.Prefix = "cache/";  // optional key prefix
});
```

## Watermark Configuration

Create an `ImageResizerJson.json` file in your `wwwroot` directory:

```json
{
  "WatermarkTextList": [
    {
      "Key": 1,
      "Value": "© 2024 MyCompany",
      "Color": "#FFFFFF",
      "TextSize": 30,
      "PositionMeasureType": 2,
      "X": 50,
      "Y": 90
    }
  ],
  "WatermarkImageList": [
    {
      "Key": 1,
      "Url": "/images/watermark.png",
      "SizeMeasureType": 2,
      "Width": 20,
      "Height": 0,
      "PositionMeasureType": 1,
      "Right": 10,
      "Bottom": 10
    }
  ]
}
```

## Performance

- **Memory caching** - Processed images are automatically cached in memory
- **Persistent caching** - Optional DiskCache or S3Cache plugins for cross-restart persistence
- **Fast processing** - Uses SkiaSharp for efficient image manipulation
- **Lightweight** - Minimal dependencies

## Supported Image Formats

- **Input**: PNG, JPG, JPEG
- **Output**: PNG, JPG, JPEG

## License

See LICENSE file for details.

## Support

- GitHub: [https://github.com/NicoJuicy/ImageResizer.AspNetCore](https://github.com/NicoJuicy/ImageResizer.AspNetCore)

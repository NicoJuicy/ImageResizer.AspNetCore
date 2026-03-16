# Sapico.ImageResizer

A lightweight, fast image processing middleware for ASP.NET Core that enables on-the-fly image resizing, cropping, format conversion, and watermarking through simple URL query parameters.

**Note:** This is a maintained fork of [ImageResizer.AspNetCore](https://github.com/cornelha/ImageResizer.AspNetCore) updated to support .NET 10.0 with added features including width/height query parameter aliases, plugin architecture for caching, and Linux native asset support.

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
- **Flexible query parameters** - Use `w`/`width` and `h`/`height` aliases (case-insensitive)
- **Plugin architecture** - Extensible caching system with IImageCache interface
- **Memory caching** - Default in-memory cache for processed images
- **Disk cache plugin** - Optional filesystem-based persistent cache (Sapico.ImageResizer.Plugin.DiskCache)
- **S3 cache plugin** - Optional S3-based distributed cache (Sapico.ImageResizer.Plugin.S3Cache)
- **Linux support** - Includes SkiaSharp native assets for Linux
- **.NET 10 support** - Targets .NET 10.0

## Installation

Install via NuGet:

```bash
dotnet add package Sapico.ImageResizer
```

Or using Package Manager Console:

```powershell
Install-Package Sapico.ImageResizer
```

## Quick Start

### 1. Register the Service

In your `Program.cs` (ASP.NET Core 6+):

```csharp
using Sapico.ImageResizer;

var builder = WebApplication.CreateBuilder(args);

// Add ImageResizer service
builder.Services.AddImageResizer();

var app = builder.Build();

// Use ImageResizer middleware
app.UseStaticFiles();
app.UseImageResizer();

app.Run();
```

Or in `Startup.cs` (ASP.NET Core 5):

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddImageResizer();
}

public void Configure(IApplicationBuilder app)
{
    app.UseStaticFiles();
    app.UseImageResizer();
}
```

### 2. Use in HTML

Simply add query parameters to your image URLs:

```html
<img src="~/images/photo.jpg?w=200" alt="Resized" />
<img src="~/images/photo.jpg?width=200&height=300" alt="Resized with height" />
```

**Note:** You can use either `w`/`h` or `width`/`height` (case-insensitive) as query parameters.

## Usage Examples

### Basic Resizing

Resize to a specific width (height auto-calculated to maintain aspect ratio):

```html
<img src="~/images/photo.jpg?w=300" alt="Width 300px" />
```

Resize to a specific height (width auto-calculated):

```html
<img src="~/images/photo.jpg?h=400" alt="Height 400px" />
```

Resize to specific width and height with auto-detection of best fit:

```html
<img src="~/images/photo.jpg?w=200&h=200" alt="Max fit" />
```

### Format Conversion

Convert to PNG:

```html
<img src="~/images/photo.jpg?format=png" alt="PNG format" />
```

Convert to JPEG with quality setting:

```html
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

Adjust compression quality (1-100, default 100):

```html
<img src="~/images/photo.jpg?w=800&quality=75" alt="Optimized" />
```

### Auto-Rotation

Enable automatic EXIF-based rotation:

```html
<img src="~/images/photo.jpg?autorotate=true" alt="Auto-rotated" />
```

### Combined Parameters

Use multiple parameters together:

```html
<!-- Resize to 500x300, crop to fill, JPEG format, 80% quality -->
<img src="~/images/photo.jpg?w=500&h=300&mode=crop&format=jpg&quality=80" alt="Optimized" />

<!-- Resize width to 400, auto-height, PNG format, auto-rotate -->
<img src="~/images/photo.jpg?w=400&format=png&autorotate=true" alt="Responsive" />
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

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `w` or `width` | int | 0 | Width in pixels (0 = auto-calculate, case-insensitive) |
| `h` or `height` | int | 0 | Height in pixels (0 = auto-calculate, case-insensitive) |
| `mode` | string | max | Resize mode: `max`, `pad`, `crop`, `stretch` |
| `format` | string | original | Output format: `jpg`, `jpeg`, `png` |
| `quality` | int | 100 | JPEG quality (1-100) |
| `autorotate` | bool | false | Auto-rotate based on EXIF orientation |
| `wmtext` | int | 0 | Text watermark ID (requires config) |
| `wmimage` | int | 0 | Image watermark ID (requires config) |

## Cache Plugins

Sapico.ImageResizer includes a plugin architecture for caching processed images. By default, it uses an in-memory cache (`MemoryImageCache`), but you can use additional cache plugins for persistence and distributed scenarios.

### Disk Cache Plugin

Install the disk cache plugin for persistent filesystem-based caching:

```bash
dotnet add package Sapico.ImageResizer.Plugin.DiskCache
```

Configure in `Program.cs`:

```csharp
using Sapico.ImageResizer;
using Sapico.ImageResizer.Plugin.DiskCache;

var builder = WebApplication.CreateBuilder(args);

// AddImageResizerDiskCache replaces AddImageResizer — no need to call both
builder.Services.AddImageResizerDiskCache(options =>
{
    options.CacheFolder = Path.Combine(builder.Environment.ContentRootPath, "cache", "images");
});

var app = builder.Build();
app.UseStaticFiles();
app.UseImageResizer();
app.Run();
```

`AddImageResizerDiskCache` already registers `IMemoryCache` and `IImageCache`, so calling `AddImageResizer` separately is not required. The `CacheFolder` defaults to `~/cache` in the user profile directory if not specified. You can also call `AddImageResizerDiskCache()` without options to use the default.

**Benefits:**
- Persistent cache survives application restarts
- Reduces processing load for frequently accessed images
- Uses in-memory + disk two-tier caching for fast lookups

### S3 Cache Plugin

Install the S3 cache plugin for distributed cloud-based caching:

```bash
dotnet add package Sapico.ImageResizer.Plugin.S3Cache
```

Configure in `Program.cs`:

```csharp
using Sapico.ImageResizer;
using Sapico.ImageResizer.Plugin.S3Cache;

var builder = WebApplication.CreateBuilder(args);

// AddImageResizerS3Cache replaces AddImageResizer — no need to call both
builder.Services.AddImageResizerS3Cache(options =>
{
    options.BucketName = "my-image-cache";
    options.Region = "us-east-1";
    options.Prefix = "resized/"; // optional key prefix in S3
    options.AccessKey = builder.Configuration["AWS:AccessKey"];
    options.SecretKey = builder.Configuration["AWS:SecretKey"];
});

var app = builder.Build();
app.UseStaticFiles();
app.UseImageResizer();
app.Run();
```

If `AccessKey`/`SecretKey` are omitted, the plugin falls back to the default AWS credential chain (environment variables, IAM role, etc.).

**Benefits:**
- Distributed caching across multiple application instances
- Scales with your cloud infrastructure
- Ideal for load-balanced or serverless deployments

### Custom Cache Plugin

You can create your own cache plugin by implementing the `IImageCache` interface:

```csharp
public interface IImageCache
{
    bool TryGet(long key, out byte[] imageBytes);
    void Set(long key, byte[] imageBytes);
}
```

Register your custom cache implementation in `Program.cs`:

```csharp
builder.Services.AddSingleton<IImageCache, MyCustomImageCache>();
```

## Watermark Configuration

To use watermarks, create an `ImageResizerJson.json` file in your `wwwroot` directory:

```json
{
  "WatermarkTextList": [
    {
      "Key": 1,
      "Text": "© 2024 MyCompany",
      "Color": "#FFFFFF",
      "Size": 30,
      "Opacity": 0.7
    }
  ],
  "WatermarkImageList": [
    {
      "Key": 1,
      "ImagePath": "/images/watermark.png",
      "Opacity": 0.5
    }
  ]
}
```

## Performance

- **Memory caching** - Default in-memory cache for processed images with configurable expiration
- **Plugin architecture** - Extensible cache system (Disk, S3, or custom implementations)
- **Fast processing** - Uses SkiaSharp for efficient image manipulation
- **Responsive** - Query parameters are parsed only when needed
- **Lightweight** - Minimal dependencies
- **Linux support** - Includes native SkiaSharp assets for Linux deployments

## Supported Image Formats

- **Input**: PNG, JPG, JPEG
- **Output**: PNG, JPG, JPEG

## Browser Compatibility

Works with all modern browsers that support standard image tags. The resizing happens server-side, so all clients can view the resized images.

## License

See LICENSE file for details.

## Support

For issues, feature requests, or questions:
- GitHub: [https://github.com/NicoJuicy/ImageResizer.AspNetCore](https://github.com/NicoJuicy/ImageResizer.AspNetCore)

## Version History

### v10.0.2 (Latest)
- **Linux native assets support** - Added SkiaSharp.NativeAssets.Linux for Linux deployments
- **.NET 10 only** - Simplified to target only .NET 10.0
- **Version bump** - Updated dependencies to latest .NET 10 packages

### v10.0.0
- **Sapico.ImageResizer fork** - Renamed from Core5.ImageResizer to Sapico.ImageResizer
- **Width/Height aliases** - Added case-insensitive `width`/`height` query parameter aliases
- **Plugin architecture** - Introduced `IImageCache` abstraction for extensible caching
- **DiskCache plugin** - Filesystem-based persistent cache
- **S3Cache plugin** - S3-based distributed cache
- **Namespace changes** - Migrated from `ImageResizer.AspNetCore` to `Sapico.ImageResizer`


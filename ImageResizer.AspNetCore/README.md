# ImageResizer.AspNetCore (Core5.ImageResizer)

A lightweight, fast image processing middleware for ASP.NET Core that enables on-the-fly image resizing, cropping, format conversion, and watermarking through simple URL query parameters.

**Note:** This is a maintained fork of [ImageResizer.AspNetCore](https://github.com/keyone2693/ImageResizer.AspNetCore) updated to support newer .NET versions (targets .NET 8.0, .NET 9.0 and .NET 10.0).

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
- **Multi-framework support** - Supports .NET 8.0, .NET 9.0 and .NET 10.0

## Installation

Install via NuGet:

```bash
dotnet add package Core5.ImageResizer
```

Or using Package Manager Console:

```powershell
Install-Package Core5.ImageResizer
```

## Quick Start

### 1. Register the Service

In your `Program.cs` (ASP.NET Core 6+):

```csharp
using ImageResizer.AspNetCore;

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
<img src="~/images/photo.jpg?w=200&h=300" alt="Resized with height" />
```

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
| `w` | int | 0 | Width in pixels (0 = auto-calculate) |
| `h` | int | 0 | Height in pixels (0 = auto-calculate) |
| `mode` | string | max | Resize mode: `max`, `pad`, `crop`, `stretch` |
| `format` | string | original | Output format: `jpg`, `jpeg`, `png` |
| `quality` | int | 100 | JPEG quality (1-100) |
| `autorotate` | bool | false | Auto-rotate based on EXIF orientation |
| `wmtext` | int | 0 | Text watermark ID (requires config) |
| `wmimage` | int | 0 | Image watermark ID (requires config) |

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

- **Memory caching** - Processed images are automatically cached in memory
- **Fast processing** - Uses SkiaSharp for efficient image manipulation
- **Responsive** - Query parameters are parsed only when needed
- **Lightweight** - Minimal dependencies

## Supported Image Formats

- **Input**: PNG, JPG, JPEG
- **Output**: PNG, JPG, JPEG

## Browser Compatibility

Works with all modern browsers that support standard image tags. The resizing happens server-side, so all clients can view the resized images.

## License

See LICENSE file for details.

## Support

For issues, feature requests, or questions:
- GitHub: [https://github.com/cornelha/ImageResizer.AspNetCore](https://github.com/cornelha/ImageResizer.AspNetCore)

# Sapico.ImageResizer - Agent Reference

## Project Identity

- **NuGet Package**: `Sapico.ImageResizer`
- **Namespace**: `Sapico.ImageResizer`
- **Target Frameworks**: net8.0, net9.0, net10.0
- **Image Library**: SkiaSharp 3.x
- **License**: MIT
- **Fork chain**: keyone2693 -> cornelha -> NicoJuicy (this fork)

## Solution Structure

```
ImageResizing.sln
├── ImageResizer.AspNetCore/          # Core library (Sapico.ImageResizer)
│   ├── ImageResizerMiddleware.cs     # ASP.NET Core middleware entry point
│   ├── Helpers/
│   │   ├── Extensions.cs            # DI registration (AddImageResizer, UseImageResizer)
│   │   └── Params.cs                # ResizeParams struct
│   ├── Funcs/
│   │   ├── Crop.cs                  # Center-crop logic
│   │   ├── Padding.cs               # Pad-to-size with background fill
│   │   ├── RotateAndFlip.cs         # EXIF orientation correction
│   │   └── Watermark.cs             # Text and image watermarking
│   └── Models/
│       ├── WatermarksModel.cs       # Root config model
│       ├── WatermarkTextModel.cs    # Text watermark config
│       └── WatermarkImageModel.cs   # Image watermark config
├── Sapico.ImageResizer.Plugin.DiskCache/   # Disk-based cache plugin
├── Sapico.ImageResizer.Plugin.S3Cache/     # S3-based cache plugin
└── TestExample/                      # ASP.NET Core sample app
```

## Architecture Decisions

### Middleware Pattern
All image processing runs as ASP.NET Core middleware (`ImageResizerMiddleware`). Requests with image extensions and recognized query parameters are intercepted; all others pass through.

### Query Parameter Design
Parameters are detected via reflection on the `ResizeParams` struct fields. Both short (`w`, `h`) and long (`width`, `height`) forms are supported, case-insensitive. Detection of `hasParams` checks if any struct field name appears as a query key.

### Resize Modes
- **max** (default): Fit within bounds, preserve aspect ratio
- **pad**: Fit + add white/transparent padding to exact dimensions
- **crop**: Center-crop to exact aspect ratio, then resize
- **stretch**: Distort to exact dimensions

### Caching Strategy
- **In-memory**: Default via `IMemoryCache`. Cache key = hash(path) + file mtime + hash(params).
- **DiskCache plugin** (`Sapico.ImageResizer.Plugin.DiskCache`): Wraps `IMemoryCache`, persists to disk. Default folder: `~/cache`.
- **S3Cache plugin** (`Sapico.ImageResizer.Plugin.S3Cache`): Wraps `IMemoryCache`, persists to S3 bucket.

Cache invalidation: Automatic when source file's `LastWriteTimeUtc` changes.

### Image Processing Pipeline
```
Request → Parse query params → Check memory/disk/S3 cache
  → Cache hit: return cached bytes
  → Cache miss:
      Load bitmap (SkiaSharp) → EXIF auto-rotate → Apply resize mode
      → Crop/Pad → Watermark text → Watermark image → Encode → Cache → Respond
```

### File Resolution
Uses `PhysicalFileProvider` rooted at `WebRootPath` (falls back to `ContentRootPath`). Semicolons in paths are stripped (ASP.NET path info handling).

### Watermark Configuration
Loaded from `wwwroot/ImageResizerJson.json`. Two lists: `WatermarkTextList` and `WatermarkImageList`. Referenced via `wmtext=<key>` and `wmimage=<key>` query params.

### Plugin Architecture
Cache plugins register their own `IMemoryCache` decorator via DI. They are separate NuGet packages:
- `Sapico.ImageResizer.Plugin.DiskCache`
- `Sapico.ImageResizer.Plugin.S3Cache`

Each plugin provides an `AddImageResizer*` extension method for `IServiceCollection`.

## Key Conventions

- All image processing functions are `internal static` in the `Sapico.ImageResizer.Funcs` namespace.
- Models are public POCOs in `Sapico.ImageResizer.Models`.
- Extension methods for DI/middleware registration are in `Sapico.ImageResizer.Helpers.Extensions`.
- `ResizeParams` is a mutable struct (fields, not properties) because the middleware mutates dimensions during processing.
- Supported input/output formats: PNG, JPG, JPEG.

## README Sync

This file is the source of truth for architectural decisions. When this file is updated, the README.md should be regenerated to reflect any changes to:
- Query parameters (names, aliases, types)
- Cache plugin configuration
- Installation instructions
- Feature list

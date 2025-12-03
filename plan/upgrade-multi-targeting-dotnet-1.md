---
goal: Add Multi-Targeting Support for .NET 8, 9, and 10
version: 1.0
date_created: 2025-12-03
last_updated: 2025-12-03
owner: Cornel Hattingh
status: 'In progress'
tags: [upgrade, multi-targeting, infrastructure, compatibility]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This implementation plan details the steps required to update the ImageResizer.AspNetCore library to support multi-targeting for all currently supported .NET versions from .NET 8 upwards (.NET 8, .NET 9, and .NET 10). The plan ensures that the correct NuGet package versions are used for each target framework to maintain full compatibility, and that all code remains compatible across all targeted frameworks.

**Currently Supported .NET Versions (as of December 2025):**
- .NET 8 (Long Term Support - supported until November 2026)
- .NET 9 (Standard Term Support - supported until May 2026)
- .NET 10 (Preview/Current)

## 1. Requirements & Constraints

-- **REQ-001**: Library must compile and function correctly on .NET 8.0, .NET 9.0, and .NET 10.0
- **REQ-002**: Each target framework must use compatible NuGet package versions
- **REQ-003**: All existing functionality (resize, crop, padding, watermark, format conversion) must work identically across all frameworks
- **REQ-004**: TestExample project must be updated to validate multi-targeting works correctly
- **REQ-005**: NuGet package must include all framework-specific assemblies when packed

- **SEC-001**: No security vulnerabilities should be introduced by using older package versions
- **SEC-002**: All package versions must be from trusted sources (Microsoft, official maintainers)

- **CON-001**: SkiaSharp 3.x has breaking changes from 2.x - must use 3.x consistently across all targets
- **CON-002**: Microsoft.Extensions.* packages follow .NET versioning - must use version-appropriate packages
- **CON-003**: `SKFilterQuality` is obsolete in SkiaSharp 3.x - code must handle this deprecation
- **CON-004**: Cannot use .NET 10-specific APIs if they don't exist in earlier frameworks

- **GUD-001**: Use conditional PackageReference elements in csproj for version-specific dependencies
- **GUD-002**: Prefer `$(TargetFramework)` conditions over individual framework checks where possible
- **GUD-003**: Document all framework-specific behaviors or limitations

-- **PAT-001**: Use MSBuild conditions pattern: `Condition="'$(TargetFramework)' == 'net8.0'"`
-- **PAT-002**: Use single-version strategy for Microsoft.Extensions.* packages (Version="10.0.0") across all target frameworks

## 2. Implementation Steps

### Phase 1: Update Project File Multi-Targeting Configuration

GOAL-001: Configure the ImageResizer.AspNetCore.csproj to target net8.0, net9.0, and net10.0

**Acceptance Criteria:**
- AC-001: **Given** the updated csproj file, **When** `dotnet restore` is executed, **Then** all target frameworks resolve dependencies without errors
AC-002: **Given** the updated csproj file, **When** `dotnet build` is executed, **Then** assemblies are produced for the three target frameworks (net8.0, net9.0, net10.0)

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| T001 | Update `<TargetFrameworks>` element in `ImageResizer.AspNetCore/ImageResizer.AspNetCore.csproj` from `net10.0` to `net8.0;net9.0;net10.0` | ✅ | 2025-12-03 |
| T002 | Add conditional `<ItemGroup>` sections for framework-specific `Microsoft.Extensions.*` package references | ✅ | 2025-12-03 |
| T003 | Configure SkiaSharp package reference to use compatible version across all frameworks (3.119.1 stable) | ✅ | 2025-12-03 |
| T004 | Update Newtonsoft.Json to use single compatible version across all frameworks (13.0.4 is compatible with all) | ✅ | 2025-12-03 |
| T005 | Update System.Configuration.ConfigurationManager with framework-specific versions | ✅ | 2025-12-03 |

### Phase 2: Configure Framework-Specific NuGet Package Versions

- GOAL-002: Ensure each target framework uses the correct compatible version of all NuGet dependencies

**Package Version Matrix:**

| Package | net8.0 | net9.0 | net10.0 |
|---------|--------|--------|---------|
| Microsoft.Extensions.Caching.Abstractions | 10.0.0 | 10.0.0 | 10.0.0 |
| Microsoft.Extensions.Caching.Memory | 10.0.0 | 10.0.0 | 10.0.0 |
| Microsoft.Extensions.FileProviders.Physical | 10.0.0 | 10.0.0 | 10.0.0 |
| Microsoft.Extensions.Hosting | 10.0.0 | 10.0.0 | 10.0.0 |
| Microsoft.Extensions.Logging.Abstractions | 10.0.0 | 10.0.0 | 10.0.0 |
| Newtonsoft.Json | 13.0.4 | 13.0.4 | 13.0.4 |
| SkiaSharp | 3.119.1 | 3.119.1 | 3.119.1 |
| System.Configuration.ConfigurationManager | 10.0.0 | 10.0.0 | 10.0.0 |

**Acceptance Criteria:**
AC-003: **Given** the single-version Microsoft.Extensions strategy, **When** building for any target framework (net8.0/net9.0/net10.0), **Then** Microsoft.Extensions.* packages resolve to version 10.0.0

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| T006 | Ensure Microsoft.Extensions.* package references use compatible single version (10.0.0) across all target frameworks | ✅ | 2025-12-03 |
| T010 | Update SkiaSharp to stable version 3.119.1 (from preview 3.119.2-preview.1) | ✅ | 2025-12-03 |
| T011 | Update Newtonsoft.Json to version 13.0.3 (compatible with all targets) | | |

### Phase 3: Verify Code Compatibility

- GOAL-003: Ensure all source code compiles and functions correctly across all target frameworks

**Acceptance Criteria:**
- AC-007: **Given** the codebase, **When** compiled for each target framework, **Then** no compilation errors or warnings related to API compatibility occur
- AC-008: **Given** SkiaSharp 3.x APIs, **When** using `SKFilterQuality`, **Then** no obsolete warnings are generated (use `SKSamplingOptions` instead)

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| T012 | Review `ImageResizerMiddleware.cs` for framework-specific API usage | ✅ | 2025-12-03 |
| T013 | Review `Extensions.cs` - verify `SKFilterQuality` usage is properly handled (note: method exists but is obsolete) | | |
| T014 | Review `Watermark.cs` - verify SKFont usage is compatible with SkiaSharp 3.x across all targets (implemented WatermarkImage) | ✅ | 2025-12-03 |
| T015 | Review all Funcs/*.cs files for API compatibility | | |
| T016 | Add `#pragma warning disable` for known obsolete warnings if needed, with documentation | | |

### Phase 4: Update TestExample Project

- GOAL-004: Update the TestExample project to support multi-targeting for validation purposes

**Acceptance Criteria:**
- AC-009: **Given** the updated TestExample project, **When** running on each target framework, **Then** image resizing operations complete successfully
- AC-010: **Given** a test image and resize parameters, **When** processed by the middleware on each framework, **Then** identical output is produced

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| T017 | Update `TestExample/TestExample.csproj` to target `net8.0;net9.0;net10.0` | ✅ | 2025-12-03 |
| T018 | Update TestExample package references for multi-targeting compatibility | | |
| T019 | Verify TestExample builds successfully for all target frameworks | | |
| T020 | Run TestExample on each framework and verify image resizing works | | |

### Phase 5: Update Package Metadata and Documentation

- GOAL-005: Update package version, release notes, and documentation to reflect multi-targeting support

**Acceptance Criteria:**
- AC-011: **Given** the packed NuGet package, **When** inspected, **Then** assemblies for net8.0, net9.0, and net10.0 are present in the lib folder
- AC-012: **Given** the README, **When** read by users, **Then** supported frameworks are clearly documented

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| T021 | Update package version in csproj (suggest: 10.1.0 or 11.0.0 for major multi-target release) | | |
| T022 | Update `<PackageReleaseNotes>` to document multi-targeting support | | |
| T023 | Update `ImageResizer.AspNetCore/README.md` to list supported frameworks | ✅ | 2025-12-03 |
| T024 | Update AssemblyVersion and FileVersion to match package version | | |

### Phase 6: Build and Validation

- GOAL-006: Perform full build and validate NuGet package output

**Acceptance Criteria:**
- AC-013: **Given** the solution, **When** `dotnet build -c Release` is executed, **Then** build completes with 0 errors for all frameworks
- AC-014: **Given** the project, **When** `dotnet pack -c Release` is executed, **Then** NuGet package contains lib/net8.0, lib/net9.0, and lib/net10.0 folders

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| T025 | Execute `dotnet restore` for the solution | | |
| T026 | Execute `dotnet build -c Debug` and verify all frameworks compile | | |
| T027 | Execute `dotnet build -c Release` and verify all frameworks compile | | |
| T028 | Execute `dotnet pack -c Release` and verify package structure | | |
| T029 | Inspect generated .nupkg file to confirm all target framework assemblies are included | | |

## 3. Alternatives

- **ALT-001**: Target only LTS versions (.NET 8) - Rejected because users on .NET 7, 9, and 10 would not be able to use the package optimally
- **ALT-002**: Use a single netstandard2.0 target - Rejected because this would not leverage framework-specific optimizations and would require older package versions
- **ALT-003**: Use floating version references (e.g., `7.0.*`) - Rejected because this could introduce unexpected breaking changes
- **ALT-004**: Create separate NuGet packages per framework - Rejected because multi-targeting is the standard approach and easier for consumers

## 4. Dependencies

-- **DEP-001**: Microsoft.Extensions.Caching.Abstractions (10.0.0)
-- **DEP-002**: Microsoft.Extensions.Caching.Memory (10.0.0)
-- **DEP-003**: Microsoft.Extensions.FileProviders.Physical (10.0.0)
-- **DEP-004**: Microsoft.Extensions.Hosting (10.0.0)
-- **DEP-005**: Microsoft.Extensions.Logging.Abstractions (10.0.0)
-- **DEP-006**: Newtonsoft.Json (13.0.4) - Compatible with all target frameworks
- **DEP-007**: SkiaSharp (3.119.1) - Compatible with .NET 6.0+ and .NET Standard 2.0
-- **DEP-008**: System.Configuration.ConfigurationManager (10.0.0)
- **DEP-009**: Microsoft.AspNetCore.App FrameworkReference - Built-in for all target frameworks

## 5. Files

- **FILE-001**: `ImageResizer.AspNetCore/ImageResizer.AspNetCore.csproj` - Primary project file requiring multi-target configuration
- **FILE-002**: `TestExample/TestExample.csproj` - Test project requiring multi-target updates
- **FILE-003**: `ImageResizer.AspNetCore/README.md` - Documentation requiring supported frameworks update
- **FILE-004**: `ImageResizer.AspNetCore/Helpers/Extensions.cs` - Contains obsolete `SKFilterQuality` usage to review
- **FILE-005**: `ImageResizer.AspNetCore/Funcs/Watermark.cs` - Contains SKFont usage to verify compatibility
- **FILE-006**: `ImageResizer.AspNetCore/ImageResizerMiddleware.cs` - Core middleware to verify API compatibility

## 6. Testing

- **TEST-001**: Build validation - Execute `dotnet build` for each target framework individually and verify success (Validates AC-001, AC-002)
- **TEST-002**: Package structure validation - Inspect .nupkg contents to verify all framework assemblies are present (Validates AC-013, AC-014)
- **TEST-003**: Runtime validation net8.0 - Run TestExample targeting net8.0 and process a test image (Validates AC-009)
- **TEST-004**: Runtime validation net9.0 - Run TestExample targeting net9.0 and process a test image (Validates AC-009)
- **TEST-005**: Runtime validation net10.0 - Run TestExample targeting net10.0 and process a test image (Validates AC-009)
- **TEST-007**: Feature parity test - Compare resized image output across all frameworks for identical results (Validates AC-010)
- **TEST-008**: Dependency resolution test - Verify correct package versions are resolved for each framework using `dotnet list package` (Validates AC-003 through AC-006)

## 7. Risks & Assumptions

- **RISK-001**: SkiaSharp 3.x may have subtle behavioral differences across target frameworks - Mitigation: Thorough runtime testing on each framework
- **RISK-002**: .NET 7 reaches end of support (May 2024) - users may expect removal - Mitigation: Document support policy, consider removing in future version
- **RISK-003**: Preview package versions (SkiaSharp 3.119.2-preview.1) may have bugs - Mitigation: Use stable version 3.119.1
- **RISK-004**: Package size increase due to multi-targeting - Mitigation: Expected and acceptable trade-off for compatibility

- **ASSUMPTION-001**: Users upgrading to this version have compatible .NET SDK installed (8.0+)
- **ASSUMPTION-002**: Microsoft.Extensions.* packages maintain backward API compatibility within major versions
- **ASSUMPTION-003**: SkiaSharp 3.119.1 is stable and compatible with all target frameworks
- **ASSUMPTION-004**: .NET 10 APIs used in the codebase are available or polyfilled in .NET 8 and 9 as well

## 8. Related Specifications / Further Reading

- [Microsoft .NET Support Policy](https://dotnet.microsoft.com/en-us/platform/support/policy)
- [Multi-targeting for NuGet Packages](https://docs.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file)
- [SkiaSharp Documentation](https://docs.microsoft.com/en-us/dotnet/api/skiasharp)
- [MSBuild Conditional Constructs](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-conditional-constructs)
- [NuGet Package Versioning](https://docs.microsoft.com/en-us/nuget/concepts/package-versioning)

## 9. Decision Records

### DR-001: CLARITY-001 - SkiaSharp version and refactor

**Date**: 2025-12-03
**Decided By**: Maintainer (user)
**Decision**: Option A — Use SkiaSharp 3.119.1 (stable) and refactor code for 3.x compatibility

**Consequences**:
- Updated `ImageResizer.AspNetCore.csproj` and `TestExample.csproj` to pin SkiaSharp 3.119.1
- Refactored `Funcs/Watermark.cs` to implement watermark image handling with SkiaSharp 3.x APIs

### DR-002: CLARITY-002 - Microsoft.Extensions versioning strategy

**Date**: 2025-12-03
**Decided By**: Maintainer (user)
**Decision**: Option B — Use single latest Microsoft.Extensions package version (10.0.0) across all TFMs

**Consequences**:
- Maintained `Microsoft.Extensions.*` package references at 10.0.0 in `ImageResizer.AspNetCore.csproj`

### DR-003: CLARITY-003 - net7.0 support

**Date**: 2025-12-03
**Decided By**: Maintainer (user)
**Decision**: Option B — Drop net7.0; target net8.0, net9.0, net10.0 only

**Consequences**:
- Updated `TargetFrameworks` in `ImageResizer.AspNetCore.csproj` and `TestExample.csproj` to net8.0;net9.0;net10.0
- Updated plan and tests accordingly

### DR-004: CLARITY-004 - WatermarkImage implementation

**Date**: 2025-12-03
**Decided By**: Maintainer (user)
**Decision**: Option A — Implement `WatermarkImage` now for all TFMs (full implementation)

**Consequences**:
- Implemented `Funcs/Watermark.cs::WatermarkImage` to resolve watermark paths, scale, position and composite watermark images onto originals


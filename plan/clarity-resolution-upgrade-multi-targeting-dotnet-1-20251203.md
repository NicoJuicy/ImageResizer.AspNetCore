# Implementation Plan Clarity Resolution

**Plan**: upgrade-multi-targeting-dotnet-1.md
**Date**: 2025-12-03
**Total Clarity Markers Found**: 0

---

No explicit `[Needs Clarity]` markers were found in the plan file. To make the plan executable and remove latent ambiguity, I identified four areas that require an explicit decision (these are suggested clarity markers). For each suggested marker I provide 3 options with pros/cons, implementation impact, dependencies, risk level, and a recommendation.

---

## Suggested Clarity Marker #1: Technical Decision - SKIASHARP VERSION & API COMPATIBILITY (CLARITY-001)

**Context**: Phase 2 / Phase 3 — plan recommends upgrading `SkiaSharp` to `3.119.1` and notes `SKFilterQuality` has obsolescence concerns.

**Needs Clarity**: Decide which `SkiaSharp` version to target and how to handle deprecated/changed SkiaSharp APIs (e.g., `SKFilterQuality`, `SKSamplingOptions`, `SKFont` differences) across net7.0 — net10.0 builds.

**Background**: The repo currently references `SkiaSharp 3.119.2-preview.1` in some projects and uses `SKFilterQuality` in `Helpers/Extensions.cs`. SkiaSharp 3.x introduces API changes; we must choose a stable version and ensure source compiles without obsolete/behavioral issues.

### Option A: Use stable SkiaSharp 3.119.1 and refactor code for 3.x compatibility

**Description**: Pin `SkiaSharp` to stable `3.119.1` for all target frameworks and refactor any code using deprecated APIs to use their modern equivalents (e.g., prefer `SKSamplingOptions` + `SKFilterMode` instead of `SKFilterQuality`, update text drawing APIs as required).

**Pros**:
- ✅ **Stability**: Uses a published, stable package with fewer unknown bugs than preview.
- ✅ **Consistency**: Same API surface across all target frameworks simplifies code maintenance.
- ✅ **Lower risk**: Avoids depending on preview features that may change.

**Cons**:
- ❌ **Refactor work**: Requires code changes in several files (Extensions.cs, Watermark.cs, possibly RotateAndFlip/padding code).
- ❌ **Potential behavioral diffs**: Small rendering differences between old and new API behaviour may require tuning.

**Implementation Impact**:
- **Effort**: Medium (2–6 hours)
- **Complexity**: Medium
- **Affected Files**: `Helpers/Extensions.cs`, `Funcs/Watermark.cs`, `Funcs/RotateAndFlip.cs`, `ImageResizerMiddleware.cs` (verification)
- **Breaking Changes**: No breaking public API changes — internal refactor only

**Dependencies**:
- SkiaSharp 3.119.1
- .NET SDKs for target frameworks

**Risk Level**: Low–Medium — stable package but requires careful testing for visual parity.

**Recommendation**: Recommended. Use stable SkiaSharp and adapt code now to reduce long-term maintenance and avoid preview bugs.

---

### Option B: Keep the preview SkiaSharp (3.119.2-preview.1) and update as needed

**Description**: Keep the current preview version and limit refactors to only what's necessary to compile.

**Pros**:
- ✅ **Minimal immediate changes**: Less refactor work now
- ✅ **Access to latest fixes/features** present in the preview

**Cons**:
- ❌ **Higher long-term risk**: Preview packages can introduce breaking changes or bugs later
- ❌ **NuGet consumers may suffer**: Using preview packages in a released library can reduce consumer confidence

**Implementation Impact**:
- **Effort**: Low (0–2 hours) now but may increase later
- **Complexity**: Low initially
- **Affected Files**: Minimal changes to ensure compilation
- **Breaking Changes**: Possible if preview later changes

**Dependencies**:
- SkiaSharp preview packages

**Risk Level**: High — previews can destabilize package consumers.

**Recommendation**: Not recommended unless there is a compelling feature in preview that the library must use.

---

### Option C: Multi-target with different SkiaSharp versions per TFM

**Description**: Use `Condition`ed `PackageReference`s so older frameworks use a SkiaSharp version compatible with them while newer frameworks use a newer SkiaSharp version.

**Pros**:
- ✅ **Max compatibility**: Tailors best-known working SkiaSharp per framework
- ✅ **Flexibility**: Allows smoothing over framework-specific API or platform constraints

**Cons**:
- ❌ **Complexity**: Requires maintaining conditional code paths and possibly compiler directives if APIs differ
- ❌ **Higher testing burden**: Need to verify behavior across each TFM separately

**Implementation Impact**:
- **Effort**: Medium–High (4–12 hours)
- **Complexity**: High — conditional code, #if TFM if needed
- **Affected Files**: Potentially many files where API surfaces differ
- **Breaking Changes**: Internal conditional code changes may introduce subtle differences

**Dependencies**:
- Multiple SkiaSharp versions across TFMs

**Risk Level**: Medium–High

**Recommendation**: Use only if necessary for platform-specific compatibility. Prefer Option A if possible.

---

### Decision: CLARITY-001

**Selected Option**: Option A — use `SkiaSharp 3.119.1` (stable) and refactor code for the modern API.

**Status**: Implemented — `ImageResizer.AspNetCore.csproj` and `TestExample.csproj` updated, `Funcs/Watermark.cs` refactored/implemented.

---

## Suggested Clarity Marker #2: Implementation Approach - Microsoft.Extensions Package Versioning (CLARITY-002)

**Context**: Phase 2 — plan lists framework-specific Microsoft.Extensions versions and suggests conditional ItemGroups.

**Needs Clarity**: Confirm approach: (A) Conditional PackageReference per TFM, (B) Use latest (10.0.0) across all TFMs, or (C) Use version ranges (`[7.0.0,8.0.0)` style) to reduce duplication.

### Option A: Conditional ItemGroups per target framework (as planned)

**Description**: Create conditional `<ItemGroup Condition=" '$(TargetFramework)' == 'net7.0' ">` entries with the exact package versions for each framework.

**Pros**:
- ✅ **Exact, predictable versions** for each TFM
- ✅ **Avoids accidental cross-framework version mismatches**
- ✅ **Best compatibility** for each framework's runtime

**Cons**:
- ❌ **More verbose csproj** with duplicated entries
- ❌ **Maintenance overhead** when adding/removing frameworks

**Implementation Impact**:
- **Effort**: Low–Medium
- **Complexity**: Low
- **Affected Files**: `ImageResizer.AspNetCore.csproj` (primary)

**Dependencies**: None beyond NuGet

**Risk Level**: Low

**Recommendation**: Recommended — explicit mapping reduces ambiguity for package consumers.

---

### Option B: Use single latest Microsoft.Extensions version across all TFMs (e.g., 10.0.0)

**Description**: Reference `Microsoft.Extensions.*` packages at the highest compatible major version (10.0.0) across all TFMs.

**Pros**:
- ✅ **Simpler csproj** with fewer conditional entries
- ✅ **Fewer package updates to track**

**Cons**:
- ❌ **Potential incompatibility** on older TFMs if APIs require newer runtime versions
- ❌ **Risk of unexpected runtime binding issues** in older target frameworks

**Implementation Impact**:
- **Effort**: Low
- **Complexity**: Low
- **Affected Files**: `ImageResizer.AspNetCore.csproj`

**Risk Level**: Medium — depends on binary compatibility across versions

**Recommendation**: Use only if verified that 10.x packages are fully compatible with net7.0 and net8.0; otherwise prefer Option A.

---

### Option C: Use floating/compatible version ranges

**Description**: Use version ranges that allow compatible versions but avoid pinning to a single version per TFM (e.g., `Version="[7.0.0,8.0.0)"`).

**Pros**:
- ✅ **Allows security and small fixes** within a major series
- ✅ **Less maintenance than per-TFM pins**

**Cons**:
- ❌ **Less deterministic** builds and potential surprises when upstream releases appear
- ❌ **Harder to reproduce builds** in CI unless lockfiles used

**Implementation Impact**:
- **Effort**: Low
- **Complexity**: Medium
- **Affected Files**: `ImageResizer.AspNetCore.csproj`

**Risk Level**: Medium

**Recommendation**: Conservative projects should prefer Option A; teams that accept some variance can consider Option C together with lockfile/CI pinning.

---

### Decision: CLARITY-002

**Selected Option**: Option B — use the single latest Microsoft.Extensions version (10.0.0) across all TFMs.

**Status**: Implemented — `ImageResizer.AspNetCore.csproj` uses Microsoft.Extensions.* 10.0.0 for all targets.

---

## Suggested Clarity Marker #3: Requirement Decision - net7.0 Support (CLARITY-003)

**Context**: Plan targets `net7.0;net8.0;net9.0;net10.0`. .NET 7 is out of official support; decide whether to keep net7.0 as a target.

**Needs Clarity**: Should the library continue to multi-target net7.0 given it is EOL relative to later versions, or drop net7.0 and only support LTS and current versions?

### Option A: Keep support for net7.0

**Description**: Continue to include `net7.0` alongside newer frameworks in `TargetFrameworks`.

**Pros**:
- ✅ **Max compatibility** — users still on net7 keep working
- ✅ **Less disruptive** for current consumers

**Cons**:
- ❌ **Maintenance burden** for an unsupported TFM (security updates not provided upstream)
- ❌ **Long-term support cost** and testing overhead

**Implementation Impact**:
- **Effort**: Low (maintain current state)
- **Complexity**: Low
- **Affected Files**: `csproj` targets, CI matrix

**Risk Level**: Low–Medium

**Recommendation**: If users need net7, keep it; otherwise drop to reduce maintenance.

---

### Option B: Drop net7.0 and target net8.0+ only

**Description**: Remove `net7.0` from `TargetFrameworks`, target only `net8.0;net9.0;net10.0`.

**Pros**:
- ✅ **Reduce testing/maintenance overhead**
- ✅ **Encourage users to move to supported frameworks**

**Cons**:
- ❌ **Breaking for net7 consumers** who cannot upgrade
- ❌ **Possible negative reception by users still on net7**

**Implementation Impact**:
- **Effort**: Low
- **Complexity**: Low
- **Affected Files**: `csproj`, README, package versioning notes

**Risk Level**: Low

**Recommendation**: Drop only if usage analytics or maintainers prefer minimized surface area; otherwise keep for a short transition period.

---

### Option C: Conditional support with deprecation policy

**Description**: Continue targeting `net7.0` but include deprecation notice in the README and release notes, plan to drop in next major release.

**Pros**:
- ✅ **Smooth migration path** for users
- ✅ **Sets expectation clearly** while maintaining compatibility for now

**Cons**:
- ❌ **Requires communication and follow-through** for deprecation timeline

**Implementation Impact**:
- **Effort**: Low
- **Complexity**: Low
- **Affected Files**: README, release notes, change log

**Risk Level**: Low

**Recommendation**: Prefer Option C if you want to be conservative and courteous to users, then drop net7 in the following major release.

---

### Decision: CLARITY-003

**Selected Option**: Option B — drop `net7.0`; target only `net8.0;net9.0;net10.0`.

**Status**: Implemented — updated `TargetFrameworks` in `ImageResizer.AspNetCore.csproj` and `TestExample.csproj` to net8.0;net9.0;net10.0.

---

## Suggested Clarity Marker #4: Feature/Implementation Gap — Image Watermarking (CLARITY-004)

**Context**: `Funcs/Watermark.cs` contains `WatermarkImage` method returning `null` — this is incomplete and may reduce feature parity.

**Needs Clarity**: Decide whether to implement image watermarking now (so it works across all TFMs), leave as TODO with acceptance tests, or remove support and document as not implemented.

### Option A: Implement `WatermarkImage` now for all TFMs

**Description**: Implement image watermarking logic using `SkiaSharp` APIs that work across all SkiaSharp 3.x-compatible TFMs.

**Pros**:
- ✅ **Completes advertised features** (watermark image)
- ✅ **Improves product parity** with original repo

**Cons**:
- ❌ **Development and testing effort** to implement and verify positioning, opacity, scaling
- ❌ **Edge-case handling** (various image formats, sizes) requires careful tests

**Implementation Impact**:
- **Effort**: Medium (4–8 hours)
- **Complexity**: Medium
- **Affected Files**: `Funcs/Watermark.cs`, tests, README examples

**Risk Level**: Medium

**Recommendation**: Implement; this improves the library and matches the README examples.

---

### Option B: Postpone implementation and mark as `NotImplemented` in README/tests

**Description**: Leave `WatermarkImage` unimplemented for now, document the limitation and add a clear TODO and tests expecting NotImplementedException or documented behavior.

**Pros**:
- ✅ **Less immediate work** — focus on multi-targeting
- ✅ **Clear communication** to users

**Cons**:
- ❌ **Feature gap** compared to existing claims
- ❌ **Users may be disappointed** if they expect image watermarking

**Implementation Impact**:
- **Effort**: Low
- **Complexity**: Low
- **Affected Files**: `Funcs/Watermark.cs` (add TODO), README updates, tests

**Risk Level**: Low

**Recommendation**: Use only if timeline constraints demand it; otherwise implement now (Option A).

---

### Option C: Implement a minimal cross-platform image watermarking (basic feature set)

**Description**: Implement basic watermarking (positioning, scaling and opacity) that covers 80% of use cases, leave advanced features (tiling, complex blending) for later.

**Pros**:
- ✅ **Good balance** between shipping functionality and effort
- ✅ **Faster delivery** than full feature set

**Cons**:
- ❌ **May need improvements later** for advanced scenarios

**Implementation Impact**:
- **Effort**: Medium (3–6 hours)
- **Complexity**: Medium
- **Affected Files**: `Funcs/Watermark.cs`, tests, README

**Risk Level**: Medium

**Recommendation**: Prefer Option C if implementation is desired but time is limited; Option A if there is time for full-feature parity.

---

### Decision: CLARITY-004

**Selected Option**: Option A — implement `WatermarkImage` now for all TFMs.

**Status**: Implemented — `Funcs/Watermark.cs` now contains an implementation that resolves watermark paths, scales, positions and composites watermark images.

---

## Summary of Decisions

| Marker ID | Category | Decision | Status |
|-----------|----------|----------|--------|
| CLARITY-001 | Technical Decision | Option A — SkiaSharp 3.119.1 + refactor | Implemented |
| CLARITY-002 | Implementation Approach | Option B — single Microsoft.Extensions version 10.0.0 | Implemented |
| CLARITY-003 | Requirement Decision | Option B — Drop net7.0; target net8+/net9/net10 | Implemented |
| CLARITY-004 | Implementation Gap | Option A — Implement WatermarkImage fully | Implemented |

---

## Next Steps (choose one of the following)

- Reply with decision lines to resolve the suggested markers in the required format (example):

```
CLARITY-001: Option A
CLARITY-002: Option A
CLARITY-003: Option C
CLARITY-004: Option C
```

- Or reply with `Custom` and provide the specific alternative you want for any marker.

The decisions above have been applied to the implementation plan and initial code changes were made (multi-targeting, SkiaSharp pinning, WatermarkImage implementation, and project package updates).

---

*Generated automatically by clarifier on 2025-12-03*
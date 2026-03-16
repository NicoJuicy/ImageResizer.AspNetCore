# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Sapico.ImageResizer.AspNetCore/Sapico.ImageResizer.AspNetCore.csproj src/Sapico.ImageResizer.AspNetCore/
COPY src/Sapico.ImageResizer.Plugin.DiskCache/Sapico.ImageResizer.Plugin.DiskCache.csproj src/Sapico.ImageResizer.Plugin.DiskCache/
COPY src/Sapico.ImageResizer.Plugin.S3Cache/Sapico.ImageResizer.Plugin.S3Cache.csproj src/Sapico.ImageResizer.Plugin.S3Cache/
COPY tests/TestExample/TestExample.csproj tests/TestExample/
RUN dotnet restore tests/TestExample/TestExample.csproj

COPY . .
RUN dotnet publish tests/TestExample/TestExample.csproj -c Release -f net10.0 -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# Install SkiaSharp system dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    libfontconfig1 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TestExample.dll"]

# Mandelbrot Deep Zoom Viewer

## Project Overview
A .NET-based deep-zoomable Mandelbrot set visualization system. Renders the Mandelbrot set using optimized F# algorithms, parallelization, and MongoDB tile caching. The web viewer uses a Google Maps-style tile system for interactive exploration.

## Architecture

Four projects in `Mandelbrot.sln`:

| Project | Lang | Purpose |
|---------|------|---------|
| **Mandelbrot** | F# | Core library: calculation, rendering, color, image generation |
| **Repository** | C# | MongoDB data layer for tile persistence |
| **MandelbrotWeb** | C# | ASP.NET Core web app serving tiles and interactive viewer |
| **MandelbrotConsole** | F# | Console app for batch tile pre-generation |

## Technology Stack
- **.NET 9.0** (all projects)
- **F#** (core algorithms), **C#** (data/web layer)
- **ASP.NET Core** (web framework)
- **MongoDB** (tile storage — required, default: `mongodb://localhost/tiles`)
- **SixLabors.ImageSharp** 3.1.11 (cross-platform image rendering, PNG output)
- **FSharp.Collections.ParallelSeq** (parallel computation)
- **xUnit + FsUnit** (testing)
- **Google Maps API v3** (frontend viewer in `CustomTiles/Index.cshtml`)

## Key Source Files

### Mandelbrot/ (F# library)
- [Mandelbrot.fs](Mandelbrot/Mandelbrot.fs) — Core set calculation; main function is `inSetWithResultUltra` (mutable loop, early cardioid/bulb escapes)
- [Graph.fs](Mandelbrot/Graph.fs) — Rendering engine; parallel pixel iteration via `PSeq.withDegreeOfParallelism ProcessorCount`
- [ColorModule.fs](Mandelbrot/ColorModule.fs) — 16-color palette; `toColor` maps iteration count to color
- [Image2.fs](Mandelbrot/Image2.fs) — `Bitmap3` wrapper around `Image<Rgba32>` with PNG export
- [MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs) — Tile generation orchestration; tile size 256×256, default 400 iterations
- [RectangleD.fs](Mandelbrot/RectangleD.fs) — Double-precision rectangle with zoom coordinate math
- [tests/](Mandelbrot/tests/) — xUnit/FsUnit tests for Graph, Mandelbrot, MapTileGenerator, RectangleD

### Repository/ (C# library)
- [TileRepository.cs](Repository/TileRepository.cs) — MongoDB CRUD for tiles (save, fetch, check existence, zoom range queries)
- [Domain/Tile.cs](Repository/Domain/Tile.cs) — Tile entity: X, Y, Zoom, TileSetName, Data (byte[]), Duration, CreatedDateTime

### MandelbrotWeb/ (ASP.NET Core)
- [Controllers/MapTileController.cs](MandelbrotWeb/Controllers/MapTileController.cs) — Route `/MapTile/Index?x=&y=&z=&tileSetName=`; 1-hour response cache; generates missing tiles on-demand
- [Views/CustomTiles/Index.cshtml](MandelbrotWeb/Views/CustomTiles/Index.cshtml) — Interactive Google Maps viewer
- [appsettings.json](MandelbrotWeb/appsettings.json) — MongoDB connection string config

## Tile System
- **Tile size**: 256×256 pixels (PNG)
- **Zoom levels**: 0–99 (OpenStreetMap/TMS-style; 2^zoom cells per axis)
- **Coordinate mapping**: Tile (x, y, zoom) → complex plane rectangle → Mandelbrot pixels
- **Storage**: MongoDB collection `tiles`, indexed on X/Y/Zoom/TileSetName

## Performance
The core algorithm uses a mutable loop with direct float math (not `System.Numerics.Complex`), achieving ~5× speedup over the previous functional approach.

| Approach | Time per tile |
|----------|--------------|
| Recursive/functional | ~6 s |
| Mutable loop (`inSetWithResultUltra`) | ~1.2 s |

See [performance.md](performance.md) for benchmarks.

## Build & Run

```bash
dotnet build        # Build solution
dotnet test         # Run tests

# Web app (requires MongoDB running)
cd MandelbrotWeb && dotnet run
# → http://localhost:5000

# Batch tile pre-generation
cd MandelbrotConsole && dotnet run
```

## Common Tasks
- **Change iteration limit**: Edit `iterations = 400` in [MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs)
- **Adjust colors**: Edit `namesColors2` array in [ColorModule.fs](Mandelbrot/ColorModule.fs)
- **Change tile cache duration**: `Duration = 3600` in [MapTileController.cs](MandelbrotWeb/Controllers/MapTileController.cs)
- **MongoDB connection**: `ConnectionStrings.MongoDb` in [appsettings.json](MandelbrotWeb/appsettings.json)

## Notes
- `MandelbrotWpf/` is a legacy WPF client — no longer actively maintained
- ImageSharp replaced System.Drawing for cross-platform support (no libgdiplus needed)
- `TileGenerator.fs` is a legacy file with most code commented out

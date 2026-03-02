# Performance

## Benchmark Results

### Before mutable loop optimisation
- Duration : 00:00:06.1127105
- Duration : 00:00:05.1794877
- Duration : 00:00:05.0982349

### After mutable loop optimisation
- Duration : 00:00:01.3510425
- Duration : 00:00:01.1963436
- Duration : 00:00:01.1922980

### After Array.Parallel.init + lock-free bitmap writes (2026-03-02)
```
Zoom 0: 1 tile(s) in 85ms (avg 85ms/tile)
Zoom 1: 4 tile(s) in 141ms (avg 35ms/tile)
Zoom 2: 16 tile(s) in 641ms (avg 40ms/tile)
Zoom 3: 64 tile(s) in 2427ms (avg 37ms/tile)
```

Run the benchmark:
```
dotnet test Mandelbrot/Mandelbrot.fsproj --filter "performance" --logger "console;verbosity=normal"
```

---

## Planned Improvements

### Critical

**1. Thundering herd — duplicate tile generation**
[MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs) has a check-then-act race: concurrent requests for the same missing tile each trigger an independent render, wasting CPU and doing redundant DB writes.

Fix: Add a `ConcurrentDictionary<string, Task<byte[]>>` generation gate so only one render per tile key ever runs concurrently.

**2. Missing MongoDB compound index** ✅ Done
Index created manually on `(TileSetName, Zoom, X, Y)`. Field order doesn't matter for equality queries so this covers all lookups in [TileRepository.cs](Repository/TileRepository.cs) efficiently. Note: created without `unique: true` — add that constraint if duplicate-tile prevention is desired.

**3. New `TileRepository` instantiated per HTTP request** ✅ Done
`TileRepository` is now registered as a singleton in [Startup.cs](MandelbrotWeb/Startup.cs) and injected into [MapTileController.cs](MandelbrotWeb/Controllers/MapTileController.cs) via the primary constructor — one shared instance and connection pool for the lifetime of the app.

### High

**4. No in-process cache — every request hits MongoDB**
Hot, recently-served tiles still pay MongoDB round-trip latency (1–5 ms) on every request.

Fix: `IMemoryCache` wrapper in front of `TileRepository` for a fast in-process path.

**5. Per-pixel locking / sequence allocations** ✅ Done
`setPixel` locked on every pixel (65,536 locks per tile); `IterateGraph` used multiple intermediate `seq` allocations. Fixed via `Array.Parallel.init` + sequential write loop. See [Graph.fs](Mandelbrot/Graph.fs) and [Image2.fs](Mandelbrot/Image2.fs).

### Medium

**6. Synchronous I/O throughout** ✅ Done
`TileRepository` now exposes `TryGetTileImageByteAsync`, `SaveAsync`, and `TryGetTileAsync` using the async MongoDB driver. `MapTileGenerator` adds `renderAsync` / `getTileImageByteAsync` via F# `task { }`. `MapTileController.Index` is now `async Task<IActionResult>`. See [TileRepository.cs](Repository/TileRepository.cs), [MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs), and [MapTileController.cs](MandelbrotWeb/Controllers/MapTileController.cs).

### Lower

**7. Zoom-aware iteration count**
All tiles use a fixed 400 iterations. Low-zoom tiles waste computation; very high-zoom tiles may lack detail.

Fix: Scale with zoom, e.g. `iterations = max 100 (zoom * 50)` in [MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs).

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

**1. Avoid `sqrt` in escape check** ✅ Done
`Complex.Abs(z) > 2.0` computes a square root on every iteration of the inner loop — the hottest path in the system. Compare squared magnitudes instead:
```fsharp
z.Real * z.Real + z.Imaginary * z.Imaginary > 4.0
```
One-line change in [Mandelbrot.fs](Mandelbrot/Mandelbrot.fs); applies to `inSetWithResult` (and `inSet`). User time: 2m44s → 2m18s.

**2. Thundering herd — duplicate tile generation**
[MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs) has a check-then-act race: concurrent requests for the same missing tile each trigger an independent render, wasting CPU and doing redundant DB writes.

Fix: Add a `ConcurrentDictionary<string, Task<byte[]>>` generation gate so only one render per tile key ever runs concurrently.

### High

**3. Cardioid / period-2 bulb pre-check**
Many pixels fall inside the main cardioid or period-2 bulb and will never escape. Detecting them analytically before the iteration loop avoids potentially thousands of iterations per pixel. CLAUDE.md references this as `inSetWithResultUltra` but it is not yet implemented:
```fsharp
let q = (cr - 0.25)*(cr - 0.25) + ci*ci
let inCardioid = q * (q + (cr - 0.25)) < 0.25 * ci * ci
let inBulb = (cr + 1.0)*(cr + 1.0) + ci*ci < 0.0625
```

**4. No in-process cache — every request hits MongoDB**
Hot, recently-served tiles still pay MongoDB round-trip latency (1–5 ms) on every request.

Fix: `IMemoryCache` wrapper in front of `TileRepository` for a fast in-process path.

### Medium

**5. `FirstOrDefaultAsync` in TileRepository**
`TryGetTileAsync` materialises a list with `.ToListAsync()` then calls `.SingleOrDefault()`. Use `.FirstOrDefaultAsync()` directly. Same applies to the sync `TryGetTile` at [TileRepository.cs:74](Repository/TileRepository.cs).

### Lower

**6. Dead code cleanup**
- `inSet` in [Mandelbrot.fs:21](Mandelbrot/Mandelbrot.fs) is never called by any render path.
- The final `else NotInSet(0)` branch in `inSetWithResult` is unreachable (after the loop, either `escaped` is true or `i >= iterationsToCheck`).

---

## Completed

**MongoDB compound index** ✅
Index created manually on `(TileSetName, Zoom, X, Y)`. Field order doesn't matter for equality queries so this covers all lookups in [TileRepository.cs](Repository/TileRepository.cs) efficiently. Note: created without `unique: true` — add that constraint if duplicate-tile prevention is desired.

**`TileRepository` instantiated per HTTP request** ✅
`TileRepository` is now registered as a singleton in [Startup.cs](MandelbrotWeb/Startup.cs) and injected into [MapTileController.cs](MandelbrotWeb/Controllers/MapTileController.cs) via the primary constructor — one shared instance and connection pool for the lifetime of the app.

**Per-pixel locking / sequence allocations** ✅
`setPixel` locked on every pixel (65,536 locks per tile); `IterateGraph` used multiple intermediate `seq` allocations. Fixed via `Array.Parallel.init` + sequential write loop. See [Graph.fs](Mandelbrot/Graph.fs) and [Image2.fs](Mandelbrot/Image2.fs).

**Synchronous I/O throughout** ✅
`TileRepository` now exposes `TryGetTileImageByteAsync`, `SaveAsync`, and `TryGetTileAsync` using the async MongoDB driver. `MapTileGenerator` adds `renderAsync` / `getTileImageByteAsync` via F# `task { }`. `MapTileController.Index` is now `async Task<IActionResult>`. See [TileRepository.cs](Repository/TileRepository.cs), [MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs), and [MapTileController.cs](MandelbrotWeb/Controllers/MapTileController.cs).

**Zoom-aware iteration count** ✅
Replaced fixed `iterations = 400` with `iterationsForZoom zoom = max 100 (zoom * 50)` in [MapTileGenerator.fs](Mandelbrot/MapTileGenerator.fs). All render paths (`render`, `renderAsync`, `toDomainTile`) now use the zoom-derived value. Also fixed a latent bug where `render` was creating the `Graph` with the global constant instead of the `iterationsToCheck` parameter.

**Mutable loop optimisation** ✅
Replaced recursive/functional Mandelbrot iteration with a mutable loop using direct float math (not `System.Numerics.Complex`), achieving ~5× speed improvement. See [Mandelbrot.fs](Mandelbrot/Mandelbrot.fs).

**Array.Parallel.init + lock-free bitmap writes** ✅
Replaced per-pixel locking with `Array.Parallel.init` for computation and a single sequential write pass. See [Graph.fs](Mandelbrot/Graph.fs).

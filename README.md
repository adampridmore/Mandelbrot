
# Mandelbrot

Various components for viewing a deepzoomable mandelbrot set.

## Components

- Mandelbrot - Module for generating tiles
- MandelbrotConsole - Generates missing tiles and saves in MongoDB
- MandelbrotWeb - Frontend for showing set in a web page. Generates missing tiles. 
- MandelbrotWpf - Ex-supported WPF client
- MongoDBScripts - Some MongoDB scripts
- Repository - Module for getting and saving image tiles.

## To run web pages

### Linux / OSX

You need https://github.com/mono/libgdiplus to run on Linux / OSX. 

To install with brew (osx)

```
brew install glib cairo libexif libjpeg giflib libtiff autoconf libtool automake pango pkg-config
brew link gettext --force
```

```
cd MandelbrotWeb
./run.sh

```

### Windows

```
cd MandelbrotWeb
run.bat

```

### All
Then open
http://localhost:5000

## Running the console app

The console app pre-generates tiles and saves them to MongoDB.

```bash
cd MandelbrotConsole
dotnet run                # render zoom levels 0–30 (default)
dotnet run -- 10          # render zoom levels 0–10
```

Requires MongoDB running at `mongodb://localhost/tiles`.

## Running the performance tests

```
dotnet test Mandelbrot/Mandelbrot.fsproj --filter "performance" --logger "console;verbosity=normal"
```

Output is printed to the console during the run, showing per-zoom tile counts, elapsed time, and average time per tile.

Example Output (2026-03-02, after Array.Parallel.init + lock-free bitmap writes)

```
Zoom 0: 1 tile(s) in 85ms (avg 85ms/tile)
Zoom 1: 4 tile(s) in 141ms (avg 35ms/tile)
Zoom 2: 16 tile(s) in 641ms (avg 40ms/tile)
Zoom 3: 64 tile(s) in 2427ms (avg 37ms/tile)
```

Example Output (2026-03-02, after zoom-aware iteration count)

```
Zoom 0: 1 tile(s) in 70ms (avg 70ms/tile)
Zoom 1: 4 tile(s) in 91ms (avg 22ms/tile)
Zoom 2: 16 tile(s) in 568ms (avg 35ms/tile)
Zoom 3: 64 tile(s) in 2128ms (avg 33ms/tile)
Total: 2875ms
```

Example Output (2026-03-02, Option B — parallel across tiles, sequential within each tile)

```
Zoom 0: 1 tile(s) in 158ms (avg 158ms/tile)
Zoom 1: 4 tile(s) in 137ms (avg 34ms/tile)
Zoom 2: 16 tile(s) in 615ms (avg 38ms/tile)
Zoom 3: 64 tile(s) in 2507ms (avg 39ms/tile)
Total: 3452ms
```

Note: zoom 0–3 has too few tiles (1–64) to fill all cores with Option B, so within-tile
parallelism (previous approach) wins here. Option B's advantage shows at zoom 4+ (256+
tiles) where tiles outnumber cores and the lack of nested parallelism pays off.

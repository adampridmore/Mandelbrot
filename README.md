
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

## Running the performance tests

```
dotnet test Mandelbrot/Mandelbrot.fsproj --filter "performance" --logger "console;verbosity=normal"
```

Output is printed to the console during the run, showing per-zoom tile counts, elapsed time, and average time per tile.

Example Output (2026-03-02)

```
Zoom 0: 1 tile(s) in 85ms (avg 85ms/tile)
Zoom 1: 4 tile(s) in 141ms (avg 35ms/tile)
Zoom 2: 16 tile(s) in 641ms (avg 40ms/tile)
Zoom 3: 64 tile(s) in 2427ms (avg 37ms/tile)
```

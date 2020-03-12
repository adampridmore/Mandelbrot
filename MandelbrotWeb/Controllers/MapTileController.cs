using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Mandelbrot;
using Repository;
using Repository.Domain;
using Microsoft.Extensions.Configuration;

namespace MandelbrotWeb.Controllers
{
    public class MapTileController : Controller
    {
        public const string TileLabelsSetName = "TileLabels";
        private readonly IConfiguration _config;
        private readonly TileRepository _tileRepository;

        public MapTileController(IConfiguration config) {
            _config = config;
            _tileRepository = Create();
        }
        
        public TileRepository Create()
        {
            var mongoUri = _config.GetConnectionString("MongoDb");
            
            return new TileRepository(mongoUri);
        }

        // Generates labled tiles.
        private FileResult LabeledTiles(int x, int y, int z)
        {
            var tileImage = CreatePlainLabeledTile(x, y, z);

            var memoryStream = new MemoryStream();

            var imageFormat = ImageFormat.Png;
            tileImage.Save(memoryStream, imageFormat);

            memoryStream.Position = 0;

            var contentType = $"image/{imageFormat.ToString().ToLowerInvariant()}";
            return new FileStreamResult(memoryStream, contentType);
        }

        public ActionResult Index(string x, string y, string z, string tileSetName)
        {
            if (string.IsNullOrWhiteSpace(tileSetName))
            {
                tileSetName = Tile.DefaultSetName;
            }

            var xVal = int.Parse(x);
            var yVal = int.Parse(y);
            var zoom = int.Parse(z);

            if (tileSetName == TileLabelsSetName)
            {
                return LabeledTiles(xVal, yVal, zoom);
            }

            return LoadFromDb(tileSetName, xVal, yVal, zoom);
        }

        private ActionResult LoadFromDb(string tileSetName, int x, int y, int zoom)
        {
            var tile = LoadTile(x, y, zoom, tileSetName);
            if (tile == null)
            {
                return NotFound();
            }

            var imageFormat = ImageFormat.Png;

            var memoryStream = new MemoryStream(tile);

            var contentType = $"image/{imageFormat.ToString().ToLowerInvariant()}";
            return new FileStreamResult(memoryStream, contentType);
        }

        private byte[] LoadTile(int x, int y, int zoom, string tileSetName)
        {
            return MapTileGenerator.getTileImageByte(x, y, zoom, tileSetName, _tileRepository);
        }

        private static Image CreatePlainLabeledTile(int x, int y, int zoom)
        {
            return TileGenerator.generateTile(x, y, zoom);
        }
    }
}
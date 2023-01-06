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

        public ActionResult Index(string x, string y, string z, string tileSetName)
        {
            if (string.IsNullOrWhiteSpace(tileSetName))
            {
                tileSetName = Tile.DefaultSetName;
            }

            var xVal = int.Parse(x);
            var yVal = int.Parse(y);
            var zoom = int.Parse(z);

            return LoadFromDb(tileSetName, xVal, yVal, zoom);
        }

        private ActionResult LoadFromDb(string tileSetName, int x, int y, int zoom)
        {
            var tile = LoadTile(x, y, zoom, tileSetName);
            if (tile == null)
            {
                return NotFound();
            }

            var imageFormat = Mandelbrot.Image2.imageTypeExtension;

            var memoryStream = new MemoryStream(tile);

            var contentType = $"image/{imageFormat.ToLowerInvariant()}";
            return new FileStreamResult(memoryStream, contentType);
        }

        private byte[] LoadTile(int x, int y, int zoom, string tileSetName)
        {
            return MapTileGenerator.getTileImageByte(x, y, zoom, tileSetName, _tileRepository);
        }
    }
}

using System.IO;
using System.Threading.Tasks;
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
            _tileRepository = new TileRepository(_config.GetConnectionString("MongoDb"));
        }

        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600)]
        public async Task<IActionResult> Index(string x, string y, string z, string tileSetName)
        {
            if (string.IsNullOrWhiteSpace(tileSetName))
                tileSetName = Tile.DefaultSetName;

            var xVal = int.Parse(x);
            var yVal = int.Parse(y);
            var zoom = int.Parse(z);

            var tile = await MapTileGenerator.getTileImageByteAsync(xVal, yVal, zoom, tileSetName, _tileRepository);
            if (tile == null)
                return NotFound();

            var contentType = $"image/{Mandelbrot.Image2.imageTypeExtension.ToLowerInvariant()}";
            return new FileStreamResult(new MemoryStream(tile), contentType);
        }
    }
}

using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using Mandelbrot;
using PaddingtonRepository;

namespace PaddingtonHeatmaps2.Controllers
{
    public class MapTileController : Controller
    {
        public const string TileLabelsSetName = "TileLabels";
        private readonly TileRepository _tileRepository = Create();
        public static TileRepository Create()
        {
            var mongoUri = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

            return new TileRepository(mongoUri);
        }

        // Generates labled tiles.
        private FileResult LabeledTiles(int x, int y, int z)
        {
            var tileImage = CreateTile(x, y, z);

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
                tileSetName = PaddingtonRepository.Domain.Tile.DefaultSetName;
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
                return HttpNotFound();
            }

            var imageFormat = ImageFormat.Png;

            var memoryStream = new MemoryStream(tile);

            var contentType = $"image/{imageFormat.ToString().ToLowerInvariant()}";
            return new FileStreamResult(memoryStream, contentType);
        }

        private byte[] LoadTile(int x, int y, int zoom, string tileSetName)
        {
            return MapTileGenerator.getTileImageByte(x, y, zoom, tileSetName);
        }
        private static Image CreateTile(int x, int y, int zoom)
        {
            return PaddingtonHeatmapsComponents.TileGenerator.generateTile(x, y, zoom);
        }
    }
}

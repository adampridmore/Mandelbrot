using System.Linq;
using System.Web.Mvc;

namespace PaddingtonHeatmaps2.Controllers
{
    public class CustomTilesController : Controller
    {
        //private readonly TileRepository _tileRepository = MapTileController.Create();

        public ActionResult Index()
        {
            //var minMaxZoomLevels = _tileRepository.GetMinMaxZoomLevels();

            //ViewBag.minZoom = minMaxZoomLevels.MinZoom;
            //ViewBag.maxZoom = minMaxZoomLevels.MaxZoom;

            var tileSetNames = PaddingtonRepository.Domain.Tile.GetTileSetNames().ToList();
            tileSetNames.Add(MapTileController.TileLabelsSetName);

            ViewBag.minZoom = 0;
            ViewBag.maxZoom = 18;
            ViewBag.tileSetNames = tileSetNames;

            return View();
        }
    }
}
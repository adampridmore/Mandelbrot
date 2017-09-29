using System.Linq;
using System.Web.Mvc;

namespace PaddingtonHeatmaps2.Controllers
{
    public class CustomTilesController : Controller
    {
        public ActionResult Index()
        {
            var tileSetNames = PaddingtonRepository.Domain.Tile.GetTileSetNames().ToList();
            tileSetNames.Add(MapTileController.TileLabelsSetName);

            ViewBag.minZoom = 0;
            ViewBag.maxZoom = 30; // Max that google support
            ViewBag.tileSetNames = tileSetNames;

            return View();
        }
    }
}
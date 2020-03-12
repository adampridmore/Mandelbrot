using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Repository.Domain;

namespace MandelbrotWeb.Controllers
{
    public class CustomTilesController : Controller
    {
        public ActionResult Index()
        {
            var tileSetNames = Tile.GetTileSetNames().ToList();
            tileSetNames.Add(MapTileController.TileLabelsSetName);

            ViewBag.minZoom = 0;
            ViewBag.maxZoom = 30; // Max that google support
            ViewBag.tileSetNames = tileSetNames;

            return View();
        }
    }
}
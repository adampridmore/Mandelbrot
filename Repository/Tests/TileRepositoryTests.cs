using System;
using NUnit.Framework;
using Repository.Domain;

namespace Repository.Tests
{
    [TestFixture]
    public class TileRepositoryTests
    {
        [SetUp]
        public void Before()
        {
            _repository.DeleteAll();
        }

        private readonly TileRepository _repository = new TileRepository("mongodb://localhost/tiles_unittests");

        [Test]
        public void CountTest()
        {
            var tileCount = _repository.CountTiles();

            Console.WriteLine(tileCount);
        }

        [Test]
        public void Delete_tileset()
        {
            var tileToKeep = new Tile
            {
                X = 1,
                Y = 2,
                Zoom = 3,
                TileSetName = "setToKeep"
            };

            var tileToDelete = new Tile
            {
                X = 2,
                Y = 3,
                Zoom = 4,
                TileSetName = "setToDelete"
            };

            _repository.Save(tileToKeep);
            _repository.Save(tileToDelete);

            _repository.DeleteTileSet("setToDelete");

            Assert.AreEqual(1, _repository.CountTiles());
            var loadTile1 = _repository.TryGetTile(tileToKeep.X, tileToKeep.Y, tileToKeep.Zoom, tileToKeep.TileSetName);
            var loadTile2 = _repository.TryGetTile(tileToDelete.X, tileToDelete.Y, tileToDelete.Zoom,
                tileToDelete.TileSetName);

            Assert.NotNull(loadTile1);
            Assert.Null(loadTile2);
        }

        [Test]
        public void ExistsTest_when_no_tile()
        {
            Assert.IsFalse(_repository.DoesTileExist(0, 0, 0, Tile.MandelbrotSetName));
        }

        [Test]
        public void ExistsTest_when_tile()
        {
            var tile = new Tile {X = 1, Y = 2, Zoom = 3, TileSetName = Tile.MandelbrotSetName};
            _repository.Save(tile);

            Assert.IsTrue(_repository.DoesTileExist(1, 2, 3, Tile.MandelbrotSetName));
        }

        [Test]
        public void MinMax_Zoom_levels()
        {
            var tile1 = new Tile
            {
                Id = "MyFilename1",
                Zoom = 3,
                TileSetName = "Test"
            };

            var tile2 = new Tile
            {
                Id = "MyFilename2",
                Zoom = 5,
                TileSetName = "Test"
            };

            _repository.Save(tile1);
            _repository.Save(tile2);

            var minMaxZoomLevels = _repository.GetMinMaxZoomLevels();

            Assert.AreEqual(3, minMaxZoomLevels.MinZoom);
            Assert.AreEqual(5, minMaxZoomLevels.MaxZoom);
        }

        [Test]
        public void MinMax_Zoom_levels_for_no_data()
        {
            var minMaxZoomLevels = _repository.GetMinMaxZoomLevels();

            Assert.AreEqual(0, minMaxZoomLevels.MinZoom);
            Assert.AreEqual(99, minMaxZoomLevels.MaxZoom);
        }

        [Test]
        public void SaveAndLoad()
        {
            var tile = new Tile
            {
                Id = "MyFilename",
                X = 1,
                Y = 2,
                Zoom = 3,
                Data = new byte[] {1, 2, 3},
                TileSetName = "TestSetName"
            };

            _repository.Save(tile);

            var loadedTile = _repository.TryGetTile(1, 2, 3, "TestSetName");

            Assert.AreEqual("MyFilename", loadedTile.Id);
            Assert.AreEqual(1, loadedTile.X);
            Assert.AreEqual(2, loadedTile.Y);
            Assert.AreEqual(3, loadedTile.Zoom);
            Assert.AreEqual("TestSetName", tile.TileSetName);
            CollectionAssert.AreEquivalent(new byte[] {1, 2, 3}, loadedTile.Data);
        }
    }
}
using System;
using Xunit;
using Repository.Domain;

namespace Repository.Tests
{
    public class TileRepositoryTests
    {
        public TileRepositoryTests()
        {
            _repository.DeleteAll();
        }
        
        private readonly TileRepository _repository = new TileRepository("mongodb://localhost/tiles_unittests");

        [Fact]
        public void CountTest()
        {
            var tileCount = _repository.CountTiles();

            Console.WriteLine(tileCount);
        }

        [Fact]
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

            Assert.Equal(1, _repository.CountTiles());
            var loadTile1 = _repository.TryGetTile(tileToKeep.X, tileToKeep.Y, tileToKeep.Zoom, tileToKeep.TileSetName);
            var loadTile2 = _repository.TryGetTile(tileToDelete.X, tileToDelete.Y, tileToDelete.Zoom,
                tileToDelete.TileSetName);

            Assert.NotNull(loadTile1);
            Assert.Null(loadTile2);
        }

        [Fact]
        public void ExistsTest_when_no_tile()
        {
            Assert.False(_repository.DoesTileExist(0, 0, 0, Tile.MandelbrotSetName));
        }

        [Fact]
        public void ExistsTest_when_tile()
        {
            var tile = new Tile {X = 1, Y = 2, Zoom = 3, TileSetName = Tile.MandelbrotSetName};
            _repository.Save(tile);

            Assert.True(_repository.DoesTileExist(1, 2, 3, Tile.MandelbrotSetName));
        }

        [Fact]
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

            Assert.Equal(3, minMaxZoomLevels.MinZoom);
            Assert.Equal(5, minMaxZoomLevels.MaxZoom);
        }

        [Fact]
        public void MinMax_Zoom_levels_for_no_data()
        {
            var minMaxZoomLevels = _repository.GetMinMaxZoomLevels();

            Assert.Equal(0, minMaxZoomLevels.MinZoom);
            Assert.Equal(99, minMaxZoomLevels.MaxZoom);
        }

        [Fact]
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

            Assert.Equal("MyFilename", loadedTile.Id);
            Assert.Equal(1, loadedTile.X);
            Assert.Equal(2, loadedTile.Y);
            Assert.Equal(3, loadedTile.Zoom);
            Assert.Equal("TestSetName", tile.TileSetName);
            Assert.Equal(new byte[] {1, 2, 3}, loadedTile.Data);
        }
    }
}
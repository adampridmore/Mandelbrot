using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Domain
{
    public class Tile : BaseEntity
    {
        public static readonly string DefaultSetName = MandelbrotSetName;
        public static readonly string MandelbrotSetName = "Mandelbrot";

        [BsonId]
        public string Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Zoom { get; set; }
        public int Iterations { get; set; }
        public byte[] Data { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string TileSetName { get; set; }

        public static IEnumerable<string> GetTileSetNames()
        {
            yield return MandelbrotSetName;
        }

        public static Tile CreateTile(int x, int y, int zoom, int iterations, string tileSetName)
        {
            return new Tile
            {
                X = x,
                Y = y,
                Zoom = zoom,
                Iterations = iterations,
                CreatedDateTime = DateTime.UtcNow,
                TileSetName = tileSetName
            };
        }
    }
}
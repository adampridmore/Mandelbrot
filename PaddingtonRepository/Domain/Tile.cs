using System;
using System.Collections.Generic;

namespace PaddingtonRepository.Domain
{
    public class Tile : BaseEntity
    {
        public static readonly string Heatmaps1SetName = "Heatmaps1"; // 10 mill - Red point
        public static readonly string Heatmaps2SetName = "Heatmaps2"; // 10 mill - Alpha blend v1
        public static readonly string Heatmaps3SetName = "Heatmaps3"; // Alpha blend v2
        public static readonly string Heatmaps4SetName = "Heatmaps4"; // 30 million AWS data Alpha Blend v2
        public static readonly string Heatmaps5DriverBehaviourEventsSetName = "Heatmaps5DriverBehaviourEvents";
        public static readonly string Heatmaps5HarshBraking = "Heatmaps5HarshBraking";

        public static readonly string MandelbrotSetName = "Mandelbrot";
        
        public static IEnumerable<string> GetTileSetNames()
        {
            yield return Heatmaps1SetName;
            //yield return Heatmaps2SetName;
            //yield return Heatmaps3SetName;
            yield return Heatmaps4SetName;
            yield return Heatmaps5DriverBehaviourEventsSetName;
            yield return Heatmaps5HarshBraking;
            yield return MandelbrotSetName;
        }
        
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Zoom { get; set; }
        public int Iterations { get; set; }
        public byte[] Data { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string TileSetName { get; set; }

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
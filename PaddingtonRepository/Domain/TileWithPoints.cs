using System.Collections.Generic;

namespace PaddingtonRepository.Domain
{
    public class TileWithPoints
    {
        public Tile Tile { get; set; }
        public List<Point> Points { get; set; }
        public string EventStatus { get; set; }
    }
}
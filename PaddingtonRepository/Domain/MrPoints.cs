using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace PaddingtonRepository.Domain
{
    public class MrPoints : BaseEntity
    {
        [BsonId]
        public Id Id { get; set; }

        [BsonElement("value")]
        public Value Value { get; set; }
    }

    
    public class Id : BaseEntity
    {
        [BsonElement("customerId")]
        public string CustomerId { get; set; }
        [BsonElement("assetId")]
        public string AssetId { get; set; }

        [BsonElement("eventStatus")]
        public string EventStatus { get; set; }

        public int Zoom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Value : BaseEntity
    {
        [BsonElement("points")]
        public List<Point> Points { get; set; }
    }
    public class Point : BaseEntity
    {
        [BsonElement("latitude")]
        public double Latitude { get; set; }
        [BsonElement("longitude")]
        public double Longitude { get; set; }
    }
}

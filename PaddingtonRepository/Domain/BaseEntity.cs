using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaddingtonRepository.Domain
{
    [BsonIgnoreExtraElements]
    public class BaseEntity
    {
        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }
}

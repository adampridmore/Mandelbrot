using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Domain;

namespace Repository
{
    public class TileRepository
    {
        private readonly IMongoCollection<Tile> _collection;
        const string CollectionName = "tiles";

        public TileRepository(string mongoUri)
        {
            var client = new MongoClient(mongoUri);

            var database = client.GetDatabase(new MongoUrl(mongoUri).DatabaseName);
            _collection = database.GetCollection<Tile>(CollectionName);
        }

        public byte[] TryGetTileImageByte(int x, int y, int zoom, string tileSetName)
        {
            return TryGetTile(x, y, zoom, tileSetName)?.Data;
        }

        public int CountTiles()
        {
            return (int) _collection.Count(FilterDefinition<Tile>.Empty);
        }

        public void Save(Tile tile)
        {
            // TODO - Check created TS is populated

            if (string.IsNullOrWhiteSpace(tile.Id))
            {
                tile.Id = ObjectId.GenerateNewId().ToString();
            }
                
                    if (string.IsNullOrWhiteSpace(tile.TileSetName))
            {
                throw new ArgumentException("Must specify a TileSetName", nameof(tile));
            }

            var query = Builders<Tile>.Filter.Eq((t=>t.Id), tile.Id);
            var options = new UpdateOptions {IsUpsert = true};
            _collection.ReplaceOne(query, tile, options);
        }

        public Tile TryGetTile(int x, int y, int zoom, string tileSetName)
        {
            var filterBuilder = Builders<Tile>.Filter;
            var filter = 
                filterBuilder.Eq("X", x) & 
                filterBuilder.Eq("Y", y) & 
                filterBuilder.Eq("Zoom", zoom) &
                filterBuilder.Eq("TileSetName", tileSetName);
            var results = _collection.Find(filter);

            return results.Limit(1).ToList().SingleOrDefault();
        }

        public bool DoesTileExist(int x, int y, int zoom, string tileSetName)
        {
            var filterBuilder = Builders<Tile>.Filter;
            var filter = 
                filterBuilder.Eq("X", x) & 
                filterBuilder.Eq("Y", y) & 
                filterBuilder.Eq("Zoom", zoom) &
                filterBuilder.Eq("TileSetName", tileSetName);
            var count = _collection.Count(filter);
            return count != 0;
        }

        public MinMaxZoomLevels GetMinMaxZoomLevels()
        {
            var result = _collection.Aggregate().Group(new BsonDocument
            {
                {"_id", BsonNull.Value},
                {"min", new BsonDocument("$min", "$Zoom")},
                {"max", new BsonDocument("$max", "$Zoom")}
            }).ToList();

            if (result.Count == 0)
            {
                return new MinMaxZoomLevels(0, 99);
            }

            var minZoom = result[0]["min"].AsInt32;
            var maxZoom = result[0]["max"].AsInt32;

            return new MinMaxZoomLevels(minZoom, maxZoom);
        }

        public void DeleteAll()
        {
            _collection.DeleteMany(FilterDefinition<Tile>.Empty);
        }

        public void DeleteTileSet(string tileSetName)
        {
            var filterBuilder = Builders<Tile>.Filter;
            var filter = filterBuilder.Eq(t=>t.TileSetName, tileSetName);
            _collection.DeleteMany(filter);
        }
    }

    public class MinMaxZoomLevels
    {
        public int MinZoom { get; }
        public int MaxZoom { get; }

        public MinMaxZoomLevels(int minZoom, int maxZoom)
        {
            MinZoom = minZoom;
            MaxZoom = maxZoom;
        }
    }
}

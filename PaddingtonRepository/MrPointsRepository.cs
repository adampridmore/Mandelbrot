using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MongoDB.Driver;
using PaddingtonRepository.Domain;

namespace PaddingtonRepository
{
    public class MrPointsRepository
    {
        private readonly IMongoCollection<MrPoints> _collection;

        //private const string CollectionName = "routeResult_mr_points";

        public MrPointsRepository(string mongoUri, string collectionName)
        {
            var client = new MongoClient(mongoUri);

            var database = client.GetDatabase(new MongoUrl(mongoUri).DatabaseName);
            _collection = database.GetCollection<MrPoints>(collectionName);
        }
        public void Save(MrPoints mrPoints)
        {
            _collection.InsertOne(mrPoints);
        }
        public void DeleteAll()
        {
            _collection.DeleteMany(FilterDefinition<MrPoints>.Empty);
        }

        public IEnumerable<TileWithPoints> GetTilesForZoom(int zoom, string eventStatusText = null)
        {
            var filterBuilder = Builders<MrPoints>.Filter;
            var filter = filterBuilder.Eq(v => v.Id.Zoom, zoom);
            if (eventStatusText != null)
            {
                filter = filter & filterBuilder.Eq(v => v.Id.EventStatus, eventStatusText);
            }

            var sort = Builders<MrPoints>.Sort
                .Ascending(v => v.Id.X)
                .Ascending(v => v.Id.Y);

            var rows = _collection
                .Find(filter)
                .Sort(sort);

            var jsonQuery = rows.ToString();
            
            TileWithPoints currentTileWithPoints = null;
            foreach (var row in rows.ToEnumerable())
            {
                if (ShouldCreateNewTile(currentTileWithPoints, row))
                {
                    if (currentTileWithPoints != null)
                    {
                        yield return currentTileWithPoints;
                    }

                    currentTileWithPoints = new TileWithPoints
                    {
                        Tile = new Tile
                        {
                            X = row.Id.X,
                            Y = row.Id.Y,
                            Zoom = row.Id.Zoom,
                        },
                        Points = new List<Point>(),
                        EventStatus = row.Id.EventStatus,
                    };
                }

                currentTileWithPoints.Points.AddRange(
                    row.Value.Points.Select(p => new Point
                    {
                        Latitude = p.Latitude,
                        Longitude = p.Longitude
                    }));
            }

            if (currentTileWithPoints != null)
            {
                yield return currentTileWithPoints;
            }
        }

        public TileWithPoints TryGetTileForZoom(int zoom, int x, int y, string eventStatusText = null)
        {
            var filterBuilder = Builders<MrPoints>.Filter;
            var filter = filterBuilder.Eq(v => v.Id.Zoom, zoom) &
                         filterBuilder.Eq(v => v.Id.X, x) &
                         filterBuilder.Eq(v => v.Id.Y, y);

            if (eventStatusText != null)
            {
                filter = filter & filterBuilder.Eq(v => v.Id.EventStatus, eventStatusText);
            }

            var rows = _collection
                .Find(filter);

            var filterJson = rows.ToString();

            TileWithPoints currentTileWithPoints = null;
            foreach (var row in rows.ToEnumerable(CancellationToken.None))
            {
                if (currentTileWithPoints == null)
                {
                    currentTileWithPoints = new TileWithPoints
                    {
                        Tile = new Tile
                        {
                            X = row.Id.X,
                            Y = row.Id.Y,
                            Zoom = row.Id.Zoom
                        },
                        Points = new List<Point>()
                    };
                }

                currentTileWithPoints.Points.AddRange(
                    row.Value.Points.Select(p => new Point
                    {
                        Latitude = p.Latitude,
                        Longitude = p.Longitude
                    }));
            }

            return currentTileWithPoints;
        }

        private static bool ShouldCreateNewTile(TileWithPoints currentTileWithPoints, MrPoints row)
        {
            if (currentTileWithPoints == null)
            {
                return true;
            }

            if (currentTileWithPoints.Tile.X != row.Id.X || currentTileWithPoints.Tile.Y != row.Id.Y)
            {
                return true;
            }

            return false;
        }
    }
}

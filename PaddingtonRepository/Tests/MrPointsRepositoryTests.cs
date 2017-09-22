using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PaddingtonRepository.Domain;

namespace PaddingtonRepository.Tests
{
    [TestFixture]
    public class MrPointsRepositoryTests
    {
        private static readonly string TestCollectionName = "mr_points_test_collection";

        readonly MrPointsRepository _repository = 
            new MrPointsRepository("mongodb://localhost/tiles_unittests", TestCollectionName);

        [SetUp]
        public void SetUp()
        {
            _repository.DeleteAll();
        }

        [Test]
        public void Go()
        {
            var mrPoints = new MrPoints
            {
                Id = new Id
                {
                    AssetId = "myAssetId",
                    CustomerId = "myCustomerId",
                    X = 1,
                    Y = 2,
                    Zoom = 3
                },
                Value = new Value
                {
                    Points = new List<Point>
                    {
                        new Point {Latitude = 53, Longitude = -2}
                    }
                }
            };

            _repository.Save(mrPoints);

            var tileWithPoints = _repository
                .GetTilesForZoom(3).ToList();

            Assert.That(tileWithPoints.Count, Is.EqualTo(1));

            Assert.That(tileWithPoints[0].Tile.Zoom, Is.EqualTo(3));
            Assert.That(tileWithPoints[0].Tile.X, Is.EqualTo(1));
            Assert.That(tileWithPoints[0].Tile.Y, Is.EqualTo(2));
            Assert.That(tileWithPoints[0].Points.Count, Is.EqualTo(1));
            Assert.That(tileWithPoints[0].Points[0].Latitude, Is.EqualTo(53));
            Assert.That(tileWithPoints[0].Points[0].Longitude, Is.EqualTo(-2));
        }
    }
}

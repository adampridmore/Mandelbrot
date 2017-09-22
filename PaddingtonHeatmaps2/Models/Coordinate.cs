namespace PaddingtonHeatmaps2.Models
{
    public class Coordinate
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        private Coordinate(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public Coordinate Create(double longitude, double latitude)
        {
            return new Coordinate(longitude, latitude);
        }
    }
}
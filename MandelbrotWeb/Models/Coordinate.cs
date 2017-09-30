namespace MandelbrotWeb.Models
{
    public class Coordinate
    {
        private Coordinate(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public Coordinate Create(double longitude, double latitude)
        {
            return new Coordinate(longitude, latitude);
        }
    }
}
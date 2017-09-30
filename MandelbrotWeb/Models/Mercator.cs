namespace MandelbrotWeb.Models
{
    public class Mercator
    {
        public double YToLatitude(double y)
        {
            return 180.0 / System.Math.PI *
                (2 *
                 System.Math.Atan(
                    System.Math.Exp(y * System.Math.PI / 180)) - System.Math.PI / 2);
        }
        public double LatitudeToY(double latitude)
        {
            return 180.0 / System.Math.PI *
                System.Math.Log(
                    System.Math.Tan(
                        System.Math.PI / 4.0 + latitude * (System.Math.PI / 180.0) / 2));
        }

    }
}
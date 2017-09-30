using System;

namespace MandelbrotWeb.Models
{
    public class Mercator
    {
        public double YToLatitude(double y)
        {
            return 180.0/Math.PI*
                   (2*
                    Math.Atan(
                        Math.Exp(y*Math.PI/180)) - Math.PI/2);
        }

        public double LatitudeToY(double latitude)
        {
            return 180.0/Math.PI*
                   Math.Log(
                       Math.Tan(
                           Math.PI/4.0 + latitude*(Math.PI/180.0)/2));
        }
    }
}
namespace MabdelbrotForm
{
    public struct Rect
    {
        public override string ToString()
        {
            return $"MinX: {MinX}, MaxX: {MaxX}, MinY: {MinY}, MaxY: {MaxY}";
        }

        public double MinX { get; }
        public double MaxX { get; }
        public double MinY { get; }
        public double MaxY { get; }

        public Rect(double minX, double maxX, double minY, double maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }
}
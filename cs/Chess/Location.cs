namespace Chess
{
    public class Location
    {
        public readonly int X, Y;

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";

        public override int GetHashCode() => unchecked((X * 397) ^ Y);

        public override bool Equals(object obj)
        {
            return obj is Location other 
                   && other.X == X 
                   && other.Y == Y;
        }
    }
}
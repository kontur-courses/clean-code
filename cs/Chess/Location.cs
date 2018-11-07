using System.Collections.Generic;

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
            var other = obj as Location;
            if (other == null) return false;
            return other.X == X && other.Y == Y;
        }

    }
}
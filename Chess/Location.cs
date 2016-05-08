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

		public bool InBoard
		{
			get { return X >= 0 && X <= 7 && Y >= 0 && Y <= 7; }
		}

		public static IEnumerable<Location> AllBoard()
		{
			for (var y = 0; y < 8; y++)
				for (var x = 0; x < 8; x++)
					yield return new Location(x, y);
		}

		public override string ToString()
		{
			return new string((char) ('a' + X), 1) + Y;
		}

		public override bool Equals(object obj)
		{
			var other = obj as Location;
			if (other == null) return false;
			return other.X == X && other.Y == Y;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X*397) ^ Y;
			}
		}
	}
}
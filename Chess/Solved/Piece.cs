using System.Collections.Generic;
using System.Linq;

namespace Chess.Solved
{
	public class Piece
	{
		public static readonly Piece Rook = new Piece(true, 'R', new Location(1, 0), new Location(0, 1));
		public static readonly Piece King = new Piece(false, 'K', new Location(1, 1), new Location(1, 0), new Location(0, 1));
		public static readonly Piece Queen = new Piece(true, 'Q', new Location(1, 1), new Location(1, 0), new Location(0, 1));
		public static readonly Piece Bishop = new Piece(true, 'B', new Location(1, 1));
		public static readonly Piece Knight = new Piece(false, 'N', new Location(2, 1), new Location(1, 2));
		private static readonly Piece[] map = new Piece[128];

		private readonly Location[] ds;
		private readonly bool infinit;

		static Piece()
		{
			foreach (var f in new[] {King, Queen, Rook, Knight, Bishop})
				map[f.Sign] = f;
			map['.'] = null;
		}

		public Piece(bool infinit, char sign, params Location[] ds)
		{
			this.infinit = infinit;
			this.ds = ds
				.Union(ds.Select(dd => new Location(-dd.X, dd.Y)))
				.Union(ds.Select(dd => new Location(dd.X, -dd.Y)))
				.Union(ds.Select(dd => new Location(-dd.X, -dd.Y)))
				.ToArray();
			Sign = sign;
		}

		public char Sign { get; private set; }

		public static Piece FromChar(char c)
		{
			return map[char.ToUpper(c)];
		}

		public IEnumerable<Location> GetMoves(Location location, Board board)
		{
			return ds.SelectMany(d => MovesInOneDirection(location, board, d, infinit));
		}

		private static IEnumerable<Location> MovesInOneDirection(Location from, Board board, Location dir, bool infinit)
		{
			var fromColoredPiece = board.Get(from);
			for (var i = 1; i < (infinit ? 8 : 2); i++)
			{
				var to = new Location(from.X + dir.X*i, from.Y + dir.Y*i);
				if (!to.InBoard) break;
				var toColoredPiece = board.Get(to);
				if (toColoredPiece.Piece == null) yield return to;
				else
				{
					if (toColoredPiece.Color != fromColoredPiece.Color) yield return to;
					yield break;
				}
			}
		}
	}
}
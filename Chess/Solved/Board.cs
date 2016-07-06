using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Solved
{
	public class Board
	{
		private readonly ColoredPiece[,] cells = new ColoredPiece[8, 8];

		public Board(string[] lines)
		{
			if (lines.Length != 8) throw new ArgumentException("Should be exactly 8 lines");
			if (lines.Any(line => line.Length != 8)) throw new ArgumentException("All lines should have 8 chars length");
			for (var y = 0; y < 8; y++)
			{
				var line = lines[y];
				if (line == null) throw new Exception("incorrect input");
				for (var x = 0; x < 8; x++)
				{
					var pieceSign = line[x];
					var color = char.IsUpper(pieceSign) ? PieceColor.White : PieceColor.Black;
					Set(new Location(x, y), new ColoredPiece(Piece.FromChar(pieceSign), color));
				}
			}
		}

		public IEnumerable<Location> GetPieces(PieceColor color)
		{
			return Location.AllBoard().Where(loc => Get(loc).Piece != null && Get(loc).Color == color);
		}

		public ColoredPiece Get(Location location)
		{
			return !location.InBoard ? ColoredPiece.Empty : cells[location.X, location.Y];
		}

		public void Set(Location location, ColoredPiece cell)
		{
			cells[location.X, location.Y] = cell;
		}

		public override string ToString()
		{
			var b = new StringBuilder();
			for (var y = 0; y < 8; y++)
			{
				for (var x = 0; x < 8; x++)
					b.Append(Get(new Location(x, y)));
				b.AppendLine();
			}
			return b.ToString();
		}

		public PieceMove PerformMove(Location from, Location to)
		{
			var old = Get(to);
			Set(to, Get(from));
			Set(from, ColoredPiece.Empty);
			return new PieceMove(this, from, to, old);
		}
	}

	public class PieceMove
	{
		private readonly Board board;
		private readonly Location from;
		private readonly ColoredPiece oldDestinationPiece;
		private readonly Location to;

		public PieceMove(Board board, Location from, Location to, ColoredPiece oldDestinationPiece)
		{
			this.board = board;
			this.from = from;
			this.to = to;
			this.oldDestinationPiece = oldDestinationPiece;
		}

		public void Undo()
		{
			board.Set(from, board.Get(to));
			board.Set(to, oldDestinationPiece);
		}
	}
}
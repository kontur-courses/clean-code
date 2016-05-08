namespace Chess
{
	public class ChessProblem
	{
		private static Board board;
		public static ChessStatus ChessStatus;

		public static void LoadFrom(string[] lines)
		{
			board = new Board(lines);
		}

		// Определяет мат, шах или пат белым.
		public static void CalculateChessStatus()
		{
			var isCheck = IsCheckForWhite();
			var hasMoves = false;
			foreach (var locFrom in board.GetPieces(PieceColor.White))
			{
				foreach (var locTo in board.Get(locFrom).Piece.GetMoves(locFrom, board))
				{
					var old = board.Get(locTo);
					board.Set(locTo, board.Get(locFrom));
					board.Set(locFrom, ColoredPiece.Empty);
					if (!IsCheckForWhite())
						hasMoves = true;
					board.Set(locFrom, board.Get(locTo));
					board.Set(locTo, old);
				}
			}
			if (isCheck)
				if (hasMoves)
					ChessStatus = ChessStatus.Check;
				else ChessStatus = ChessStatus.Mate;
			else if (hasMoves) ChessStatus = ChessStatus.Ok;
			else ChessStatus = ChessStatus.Stalemate;
		}

		private static bool IsCheckForWhite()
		{
			var isCheck = false;
			foreach (var loc in board.GetPieces(PieceColor.Black))
			{
				var cell = board.Get(loc);
				var moves = cell.Piece.GetMoves(loc, board);
				foreach (var destination in moves)
				{
					if (board.Get(destination).Is(PieceColor.White, Piece.King))
						isCheck = true;
				}
			}
			if (isCheck) return true;
			return false;
		}
	}
}
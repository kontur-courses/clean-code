using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Solved
{
	public class ChessProblem2
	{
		private readonly Board board;

		public ChessProblem2(Board board)
		{
			this.board = board;
		}

		public ChessStatus GetStatusFor(PieceColor color)
		{
			var isCheck = IsCheckFor(color);
			var hasMoves = HasSafeMovesFor(color);
			if (isCheck)
				return hasMoves ? ChessStatus.Check : ChessStatus.Mate;
			else
				return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
		}

		private bool IsCheckFor(PieceColor color)
		{
			var enemyColor = color == PieceColor.Black
				? PieceColor.White : PieceColor.Black;
			return GetAllMovesOf(enemyColor)
				.Any(m => board.Get(m.To).Is(color, Piece.King));

		}

		private bool HasSafeMovesFor(PieceColor color)
		{
			return GetAllMovesOf(color)
				.Any(m => IsSafeMove(m.From, m.To));
		}

		private IEnumerable<Move> GetAllMovesOf(PieceColor color)
		{
			return board.GetPieces(color).SelectMany(GetMoves);
		}

		private IEnumerable<Move> GetMoves(Location pieceLoc)
		{
			return board.Get(pieceLoc)
				.Piece.GetMoves(pieceLoc, board)
				.Select(destination => new Move(pieceLoc, destination));
		}

		private bool IsSafeMove(Location from, Location to)
		{
			var color = board.Get(from).Color;
			var move = board.MakeTemporaryMove(from, to);
			try
			{
				return !IsCheckFor(color);
			}
			finally
			{
				move.Undo();
			}
		}
	}

	internal class Move
	{
		public readonly Location From;
		public readonly Location To;

		public Move(Location @from, Location to)
		{
			From = @from;
			To = to;
		}
	}
}
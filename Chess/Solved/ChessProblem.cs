using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Solved
{
    public class ChessProblem
    {
        private readonly Board board;

        public ChessProblem(Board board)
        {
            this.board = board;
        }

        public ChessStatus GetStatusFor(PieceColor color)
        {
            var isCheck = IsCheckFor(color);
            var hasMoves = HasMovesFor(color, IsSafeMove);
            if (isCheck)
                return hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            else
                return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
        }

        private bool IsCheckFor(PieceColor color)
        {
            var enemyColor = color == PieceColor.Black
                ? PieceColor.White : PieceColor.Black;
            return HasMovesFor(enemyColor,
                (from, to) => board.Get(to).Is(color, Piece.King));
        }

        private bool HasMovesFor(PieceColor color, Func<Location, Location, bool> isAcceptableMove)
        {
            var moves =
                from pieceLoc in board.GetPieces(color)
                from destLoc in GetMoves(pieceLoc)
                where isAcceptableMove(pieceLoc, destLoc)
                select true;
            return moves.Any();
        }

        private IEnumerable<Location> GetMoves(Location pieceLoc)
        {
            return board.Get(pieceLoc).Piece.GetMoves(pieceLoc, board);
        }

        private bool IsSafeMove(Location from, Location to)
        {
            var color = board.Get(from).Color;
            var move = board.PerformMove(from, to);
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
}
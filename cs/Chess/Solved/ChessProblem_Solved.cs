using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Solved
{
    public class ChessProblem_Solved
    {
        private readonly Board board;

        public ChessProblem_Solved(Board board)
        {
            this.board = board;
        }

        public ChessStatus GetStatusFor(PieceColor color)
        {
            var isCheck = IsCheckFor(color);
            var hasMoves = HasMovesFor(color, IsValidMove);
            if (isCheck)
                return hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            else
                return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
        }

        private bool IsCheckFor(PieceColor color) =>
            HasMovesFor(color.Invert(),
                (from, to) => Piece.Is(board.GetPiece(to), color, PieceType.King));

        private bool HasMovesFor(PieceColor color,
            Func<Location, Location, bool> isAcceptableMove)
        {
            var moves =
                from pieceLoc in board.GetPieces(color)
                from destLoc in GetMoves(pieceLoc)
                where isAcceptableMove(pieceLoc, destLoc)
                select true;
            return moves.Any();
        }

        private IEnumerable<Location> GetMoves(Location pieceLoc) => 
            board.GetPiece(pieceLoc).GetMoves(pieceLoc, board);

        private bool IsValidMove(Location from, Location to)
        {
            var pieceColor = board.GetPiece(from).Color;
            var move = board.PerformTemporaryMove(from, to);
            var isValid = !IsCheckFor(pieceColor);
            move.Undo();
            return isValid;
        }
    }

    public static class PieceColorExtensions
    {
        public static PieceColor Invert(this PieceColor color) => 
            color == PieceColor.Black ? PieceColor.White : PieceColor.Black;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Solved
{
    public class ChessProblem_Solved2
    {
        private readonly Board board;

        public ChessProblem_Solved2(Board board)
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

        private bool IsCheckFor(PieceColor color) =>
            GetAllMovesOf(color.Invert())
                .Any(m => Piece.Is(board.GetPiece(m.To), color, PieceType.King));


        private bool HasSafeMovesFor(PieceColor color) =>
            GetAllMovesOf(color).Any(IsSafeMove);

        private IEnumerable<ChessMove> GetAllMovesOf(PieceColor color) =>
            board.GetPieces(color).SelectMany(GetMoves);

        private IEnumerable<ChessMove> GetMoves(Location pieceLoc) =>
            board.GetPiece(pieceLoc)
                .GetMoves(pieceLoc, board)
                .Select(destination => new ChessMove(pieceLoc, destination));

        private bool IsSafeMove(ChessMove move)
        {
            var pieceColor = board.GetPiece(move.From).Color;
            using (board.PerformTemporaryMove(move.From, move.To))
                return !IsCheckFor(pieceColor);
        }
    }

    public class ChessMove
    {
        public readonly Location From;
        public readonly Location To;

        public ChessMove(Location from, Location to)
        {
            From = from;
            To = to;
        }
    }
}
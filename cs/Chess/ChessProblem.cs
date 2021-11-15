using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class ChessProblem
    {
        private static Board board;
        public static ChessStatus ChessStatus;

        public static void LoadFrom(string[] lines)
        {
            board = BoardParser.Parse(lines);
        }

        public static void SetNewChessStatus(PieceColor color)
        {
            var isCheck = IsCheckFor(color);
            var hasMoves = board
                .GetPieces(color)
                .Any(locFrom => HasMoves(locFrom, color));

            ChessStatus = GetStatus(isCheck, hasMoves);
        }

        private static bool HasMoves(Location locFrom, PieceColor color)
        {
            return GetMovesByLocation(locFrom)
                .Any(locTo => IsMoveValid(locFrom, locTo, color));
        }

        private static bool IsMoveValid(Location locFrom, Location locTo, PieceColor color)
        {
            var hasMoves = false;
            var old = board.GetPiece(locTo);
            ShiftPiece(locTo, locFrom, null);
            if (!IsCheckFor(color))
                hasMoves = true;
            ShiftPiece(locFrom, locTo, old);
            return hasMoves;
        }

        private static void ShiftPiece(Location locTo, Location locFrom, Piece replacement)
        {
            board.Set(locTo, board.GetPiece(locFrom));
            board.Set(locFrom, replacement);
        }

        private static ChessStatus GetStatus(bool isCheck, bool hasMoves)
        {
            if (isCheck)
                return hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
        }

        // check — это шах
        private static bool IsCheckFor(PieceColor color)
        {
            var otherColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            return board
                .GetPieces(otherColor)
                .SelectMany(GetMovesByLocation)
                .Any(loc => IsKing(loc, color));
        }

        private static IEnumerable<Location> GetMovesByLocation(Location loc)
        {
            return board.GetPiece(loc).GetMoves(loc, board);
        }

        private static bool IsKing(Location destination, PieceColor color)
        {
            return Piece.Is(board.GetPiece(destination), color, PieceType.King);
        }
    }
}
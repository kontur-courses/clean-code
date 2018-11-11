using System.Collections;
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
            board = new BoardParser().ParseBoard(lines);
        }

        // Определяет мат, шах или пат белым.
        public static void CalculateChessStatus()
        {
            var isCheck = IsCheckFor(PieceColor.White);
            var hasMoves = false;
            foreach (var locFrom in board.GetPiecesLocations(PieceColor.White))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    var old = board.GetPiece(locTo);
                    board.Set(locTo, board.GetPiece(locFrom));
                    board.Set(locFrom, null);
                    if (!IsCheckFor(PieceColor.White))
                        hasMoves = true;
                    board.Set(locFrom, board.GetPiece(locTo));
                    board.Set(locTo, old);
                }
            }
            if (isCheck)
                ChessStatus = hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            else if (hasMoves) ChessStatus = ChessStatus.Ok;
            else ChessStatus = ChessStatus.Stalemate;
        }

        // check — это шах
        private static bool IsCheckFor(PieceColor pieceColor)
        {
            foreach (var destination in GetAllMoves(pieceColor.Opposite()))
            {
                if (Piece.Is(board.GetPiece(destination),
                    pieceColor, PieceType.King))
                    return true;
            }
            
            return false;
        }

        private static IEnumerable<Location> GetAllMoves(PieceColor color)
        {
            foreach (var pieceLocation in board.GetPiecesLocations(color))
            {
                var piece = board.GetPiece(pieceLocation);
                foreach (var move in piece.GetMoves(pieceLocation, board))
                    yield return move;
            }
        }
    }
}
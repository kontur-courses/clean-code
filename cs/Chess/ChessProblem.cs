using System;
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
            var hasMoves = HasMoves();
            if (IsCheckForWhite())
                ChessStatus = hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            else if (hasMoves) ChessStatus = ChessStatus.Ok;
            else ChessStatus = ChessStatus.Stalemate;
        }

        private static bool HasMoves()
        {
            var hasMoves = false;
            foreach (var locFrom in board.GetPieces(PieceColor.White))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    var old = board.GetPiece(locTo);
                    board.Set(locTo, board.GetPiece(locFrom));
                    board.Set(locFrom, null);
                    if (!IsCheckForWhite())
                        hasMoves = true;
                    board.Set(locFrom, board.GetPiece(locTo));
                    board.Set(locTo, old);
                }
            }
            return hasMoves;
        }
        
        

        // check — это шах
        private static bool IsCheckForWhite()
        {
            return (from loc in board.GetPieces(PieceColor.Black)
                let piece = board.GetPiece(loc)
                where piece.GetMoves(loc, board).Any(destination =>
                    Piece.Is(board.GetPiece(destination), PieceColor.White, PieceType.King))
                select loc).Any();
        }
        
        private static void WithAct()
        {
            
        }
    }
}
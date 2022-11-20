using System.Data;

namespace Chess
{
    public class ChessProblem
    {
        private static Board board;

        public static ChessStatus ChessStatus
        {
            get
            {
                return CalculateChessStatus(PieceColor.White);
            }
        }

        public static void LoadFrom(string[] lines)
        {
            board = new BoardParser().ParseBoard(lines);
        }

        // Определяет мат, шах или пат белым.
        public static ChessStatus CalculateChessStatus(PieceColor color)
        {
            var isCheck = IsCheckFor(color);
            var hasMoves = false;
            foreach (var locFrom in board.GetPieces(PieceColor.White))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    var tempMove = board.PerformTemporaryMove(locFrom, locTo);
                    if (!IsCheckFor(color))
                        hasMoves = true;
                    tempMove.Undo();
                }
            }
            return GetNewChessStatus(isCheck, hasMoves);
            
        }

        private static ChessStatus GetNewChessStatus(bool isCheck, bool hasMoves)
        {
            if (isCheck)
            {
                if (hasMoves)
                    return ChessStatus.Check;
                return ChessStatus.Mate;
            }
            else
            {
                if (hasMoves) 
                    return ChessStatus.Ok;
                return ChessStatus.Stalemate;
            }
                
        }

        // check — это шах
        private static bool IsCheckFor(PieceColor color)
        {
            foreach (var loc in board.GetPieces(color))
            {
                var piece = board.GetPiece(loc);
                var moves = piece.GetMoves(loc, board);
                foreach (var destination in moves)
                {
                    if (Piece.Is(board.GetPiece(destination),
                                 PieceColor.White, PieceType.King))
                        return true;
                }
            }
            return false;
        }
    }
}
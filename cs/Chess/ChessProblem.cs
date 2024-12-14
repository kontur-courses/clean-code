using System.Drawing;

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
            var isCheck = IsCheck(PieceColor.White);
            var hasMoves = false;
            foreach (var locFrom in board.GetPieces(PieceColor.White))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    var previousPosition = board.GetPiece(locTo);
                    using var tempMove = board.PerformTemporaryMove(locFrom, locTo);
                    if (!IsCheck(PieceColor.White))
                        hasMoves = true;
                }
            }
            SetChessStatus(isCheck, hasMoves);
        }

        private static void SetChessStatus(bool isCheck, bool hasMoves){
            if (isCheck)
                if (hasMoves)
                    ChessStatus = ChessStatus.Check;
                else ChessStatus = ChessStatus.Mate;
            else if (hasMoves) ChessStatus = ChessStatus.Ok;
            else ChessStatus = ChessStatus.Stalemate;
        }

        // check — это шах
        private static bool IsCheck(PieceColor color)
        {
            var oppositeColor = (color == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            foreach (var locFrom in board.GetPieces(oppositeColor))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    if (Piece.Is(board.GetPiece(locTo),
                                 color, PieceType.King))
                        return true;
                }
            }
            return false;
        }
    }
}
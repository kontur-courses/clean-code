using System.Collections;

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
            var isCheck = IsCheckForWhite();
            var hasMoves = false;
            foreach (var locFrom in board.GetPieces(PieceColor.White))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    var move = board.PerformTemporaryMove(locFrom, locTo);
                    if (!IsCheckForWhite())
                        hasMoves = true;
                    move.Undo();
                }
            }

            if (isCheck)
                if (hasMoves)
                    ChessStatus = ChessStatus.Check;
                else ChessStatus = ChessStatus.Mate;
            else if (hasMoves) ChessStatus = ChessStatus.Ok;
            else ChessStatus = ChessStatus.Stalemate;
        }

        // check — это шах
        private static bool IsCheckForWhite()
        {
            var isCheck = false;
            foreach (var loc in board.GetPieces(PieceColor.Black))
            {
                foreach (var destination in board.GetPiece(loc).GetMoves(loc, board))
                {
                    if (Piece.Is(board.GetPiece(destination),
                            PieceColor.White, PieceType.King))
                        return true;
                }
            }

            return isCheck;
        }
    }
}
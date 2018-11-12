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
                    var old = board.GetPiece(locTo);
                    Exchange(locTo, locFrom, board.GetPiece(locFrom), null);
                    if (!IsCheck(PieceColor.White))
                        hasMoves = true;
                    Exchange(locFrom, locTo, board.GetPiece(locTo), old);
                }
            }
            if (isCheck)
                if (hasMoves)
                    ChessStatus = ChessStatus.Check;
                else ChessStatus = ChessStatus.Mate;
            else if (hasMoves) ChessStatus = ChessStatus.Ok;
            else ChessStatus = ChessStatus.Stalemate;
            ChessStatus = hasMoves ? isCheck ? ChessStatus.Check : ChessStatus.Mate : ChessStatus.Stalemate
        }

        private static  void Exchange(Location locFrom, Location locTo, Piece old, Piece newPiece)
        {
            board.Set(locTo, newPiece);
            board.Set(locFrom, old);
        }

        // check — это шах
        private static bool IsCheck(PieceColor color)
        {
            var otherColor = (color == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            var isCheck = false;
            foreach (var loc in board.GetPieces(otherColor))
            {
                var piece = board.GetPiece(loc);
                var moves = piece.GetMoves(loc, board);
                foreach (var destination in moves)
                {
                    if (Piece.Is(board.GetPiece(destination),
                                 color, PieceType.King))
                        isCheck = true;
                }
            }

            return isCheck;
        }
    }
}
namespace Chess
{
    public class ChessProblem
    {
        private static Board _board;
        public static ChessStatus ChessStatus;

        public static void LoadFrom(string[] lines)
        {
            _board = new BoardParser().ParseBoard(lines);
        }

        // Определяет мат, шах или пат белым.
        public static void CalculateChessStatus()
        {
            var isCheck = IsCheckForWhite();
            var hasMoves = false;
            foreach (var locFrom in _board.GetPieces(PieceColor.White))
            {
                foreach (var locTo in _board.GetPiece(locFrom).GetMoves(locFrom, _board))
                {
                    var old = _board.GetPiece(locTo);
                    _board.Set(locTo, _board.GetPiece(locFrom));
                    _board.Set(locFrom, null);
                    if (!IsCheckForWhite())
                        hasMoves = true;
                    _board.Set(locFrom, _board.GetPiece(locTo));
                    _board.Set(locTo, old);
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
            foreach (var loc in _board.GetPieces(PieceColor.Black))
            {
                var piece = _board.GetPiece(loc);
                var moves = piece.GetMoves(loc, _board);
                foreach (var destination in moves)
                {
                    if (Piece.Is(_board.GetPiece(destination),
                                 PieceColor.White, PieceType.King))
                        isCheck = true;
                }
            }
            if (isCheck) return true;
            return false;
        }
    }
}
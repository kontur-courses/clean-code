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
        public static void CalculateChessStatus(PieceColor playerColor = PieceColor.White)
        {
            var isCheck = IsCheck(playerColor);
            var hasMoves = false;
            foreach (var startLocation in board.GetPiecesPositions(playerColor))
            {
                foreach (var nextPossibleLocation in board.GetPiece(startLocation).GetMoves(startLocation, board))
                {
                    var old = board.GetPiece(nextPossibleLocation);
                    board.Set(nextPossibleLocation, board.GetPiece(startLocation));
                    board.Set(startLocation, null);
                    if (!IsCheck(playerColor))
                        hasMoves = true;
                    board.Set(startLocation, board.GetPiece(nextPossibleLocation));
                    board.Set(nextPossibleLocation, old);
                }
            }

            if (isCheck)
                ChessStatus = hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            else
                ChessStatus = hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
        }

        // check — это шах
        private static bool IsCheck(PieceColor playerColor)
        {
            var opponentColor = playerColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
            var isCheck = false;
            foreach (var loc in board.GetPiecesPositions(opponentColor))
            {
                var piece = board.GetPiece(loc);
                var moves = piece.GetMoves(loc, board);
                foreach (var destination in moves)
                {
                    if (Piece.Is(board.GetPiece(destination), playerColor, PieceType.King))
                        isCheck = true;
                }
            }
            if (isCheck) return true;
            return false;
        }
    }
}
namespace Chess
{
    public class ChessProblem
    {
        private Board board;
        //public static ChessStatus ChessStatus;

        public ChessProblem(Board board)
        {
            this.board = board;
        }

        // Определяет мат, шах или пат белым.
        public ChessStatus CalculateChessStatus()
        {
            var isCheck = IsCheckFor(PieceColor.White);
            var hasMoves = false;
            foreach (var locFrom in board.GetPieces(PieceColor.White))
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
                if (hasMoves) return ChessStatus.Check;
                else return ChessStatus.Mate;
            else if (hasMoves) return ChessStatus.Ok;
            else return ChessStatus.Stalemate;
        }

        // check — это шах
        private bool IsCheckFor(PieceColor color)
        {
            var isCheck = false;
            foreach (var loc in board.GetPieces(Invert(color)))
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
            if (isCheck) return true;
            return false;
        }

        public PieceColor Invert(PieceColor color)
        {
            return color == PieceColor.Black ? PieceColor.White : PieceColor.Black;
        }
    }
}
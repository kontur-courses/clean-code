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
                    var old = board.GetPiece(locTo);
                    board.Set(locTo, board.GetPiece(locFrom));
                    board.Set(locFrom, null);
                    if (!IsCheckForWhite())
                        hasMoves = true;
                    board.Set(locFrom, board.GetPiece(locTo));
                    board.Set(locTo, old);
                }
            }

            ChessStatus = UpdateChessStatus(isCheck, hasMoves);
        }

        private static ChessStatus UpdateChessStatus(bool isCheck, bool hasMoves)
        {
            if (isCheck)
                if (hasMoves)
                    return ChessStatus.Check;
                else return ChessStatus.Mate;

            if (hasMoves) return ChessStatus.Ok;

            return  ChessStatus.Stalemate;
        }

        // check — это шах
        private static bool IsCheckForWhite()
        {
            var isCheck = false;
            foreach (var loc in board.GetPieces(PieceColor.Black))
            {
                isCheck = IsCheck(loc, isCheck);
            }

            return isCheck;
        }


        private static bool IsCheck(Location loc, bool isCheck)
        {
            var piece = board.GetPiece(loc);
            var moves = piece.GetMoves(loc, board);
            foreach (var destination in moves)
            {
                if (IsKingInLocation(destination))
                    isCheck = true;
            }

            return isCheck;
        }

        private static bool IsKingInLocation(Location destination)
        {
            return Piece.Is(board.GetPiece(destination), PieceColor.White, PieceType.King);
        }
    }
}
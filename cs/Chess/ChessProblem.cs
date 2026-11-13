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
            var isCheck = IsCheckForWhite();
            var hasMoves = false;
            foreach (var locFrom in board.GetPieces(PieceColor.White))
            {
                hasMoves = (board.GetPiece(locFrom).GetMoves(locFrom, board)).Any(x => IsHasMoveToUncheck(locFrom, x));
                if (hasMoves) {break; }
                //foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                //{
                //    //var old = board.GetPiece(locTo);
                //    //board.Set(locTo, board.GetPiece(locFrom));
                //    //board.Set(locFrom, null);
                //    //if (!IsCheckForWhite())
                //    //    hasMoves = true;
                //    //board.Set(locFrom, board.GetPiece(locTo));
                //    //board.Set(locTo, old);
                //}
            }
            //if (isCheck)
            //    if (hasMoves)
            //        ChessStatus = ChessStatus.Check;
            //    else ChessStatus = ChessStatus.Mate;
            //else if (hasMoves) ChessStatus = ChessStatus.Ok;
            //else ChessStatus = ChessStatus.Stalemate;

            ChessStatus = isCheck
            ? (hasMoves ? ChessStatus.Check : ChessStatus.Mate)
            : (hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate);
        }

        private static bool IsHasMoveToUncheck(Location locFrom, Location locTo)
        {
            var hasMoves = false;
            var temp = board.PerformTemporaryMove(locFrom, locTo);
            if (!IsCheckForWhite())
                hasMoves = true;
            temp.Undo();
            return hasMoves;
        }

        // check — это шах
        private static bool IsCheckForWhite()
        {
            var isCheck = false;
            foreach (var loc in board.GetPieces(PieceColor.Black))
            {
                var piece = board.GetPiece(loc);
                var moves = piece.GetMoves(loc, board);
                foreach (var destination in moves)
                {
                    if (Piece.Is(board.GetPiece(destination),
                                 PieceColor.White, PieceType.King))
                        isCheck = true;
                }
            }
            return isCheck;
        }
    }
}
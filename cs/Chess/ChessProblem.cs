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
        public static void CalculateChessStatusForWhite()
        {
            var isCheck = IsCheckForWhite();
            var hasMoves = false;

            foreach (var locFrom in board.GetPieces(PieceColor.White))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    using var temporaryMove = board.PerformTemporaryMove(locFrom, locTo);
                    
                    if (!IsCheckForWhite())
                    {
                        hasMoves = true;
                        break;
                    }
                }
                
                if (hasMoves)
                    break;
            }
            
            SetChessStatus(isCheck, hasMoves);
        }

        private static void SetChessStatus(bool isCheck, bool hasMoves)
        {
            if (isCheck)
            {
                ChessStatus = hasMoves
                    ? ChessStatus.Check
                    : ChessStatus.Mate;
            }
            else if (hasMoves)
            {
                ChessStatus = ChessStatus.Ok;
            }
            else
            {
                ChessStatus = ChessStatus.Stalemate;
            }
        }

        private static bool IsCheckForWhite()
        {
            foreach (var location in board.GetPieces(PieceColor.Black))
            {
                var piece = board.GetPiece(location);
                var moves = piece.GetMoves(location, board);
                
                if (moves.Any(destination => Piece.Is(board.GetPiece(destination),
                    PieceColor.White, PieceType.King)))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
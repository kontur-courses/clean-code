using System.Linq;

namespace Chess
{
    public class ChessProblem
    {
        private Board board;
        public ChessStatus ChessStatus;

        public void LoadFrom(string[] lines)
        {
            board = new BoardParser().ParseBoard(lines);
        }

        // Определяет мат, шах или пат для заданного цвета.
        public void CalculateChessStatus(PieceColor color)
        {
            var isCheck = IsCheck(color);
            var hasMoves = HasMoves(color);

            ChessStatus = FindStatus(isCheck, hasMoves);
        }

        private ChessStatus FindStatus(bool isCheck, bool hasMoves)
        {
            if (isCheck)
            {
                return hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            }
            return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
        }

        private bool HasMoves(PieceColor color)
        {
            return board.GetPieces(color)
                .Any(locFrom => HasMoves(color, locFrom));
        }

        private bool HasMoves(PieceColor color, Location locFrom)
        {
            var hasMoves = false;
            foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
            {
                var old = board.GetPiece(locTo);
                board.Set(locTo, board.GetPiece(locFrom));
                board.Set(locFrom, null);
                if (!IsCheck(color))
                    hasMoves = true; // Нужно сначала вернуть состояние, а потом выходить
                board.Set(locFrom, board.GetPiece(locTo));
                board.Set(locTo, old);
            }

            return hasMoves;
        }

        // check — это шах
        private bool IsCheck(PieceColor color)
        {
            foreach (var loc in board.GetPieces(GetOppositePieceColor(color)))
            {
                var piece = board.GetPiece(loc);
                var moves = piece.GetMoves(loc, board);
                if (moves
                    .Any(destination => Piece.Is(board.GetPiece(destination),
                        color, PieceType.King)))
                {
                    return true;
                }
            }
            
            return false;
        }

        private PieceColor GetOppositePieceColor(PieceColor color)
        {
            return (PieceColor)(PieceColor.White - color);
        }
    }
}
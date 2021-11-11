using System.Diagnostics;
using System.Linq;

namespace Chess
{
    public class ChessProblem
    {
        private static Board board;
        public static ChessStatus ChessStatus;

        public static void SetBoard(Board board)
        {
            ChessProblem.board = board;
        }

        public static void LoadFrom(string[] lines)
        {
            SetBoard(new BoardParser().ParseBoard(lines));
        }

        public static void CalculateChessStatus(PieceColor color)
        {
            UpdateChessStatus(IsCheckFor(color), HasMoves(color));
        }

        private static bool HasMoves(PieceColor color)
        {
            return board.GetPieces(color).Any(locationFrom => CanAvoidCheck(color, locationFrom));
        }

        private static void UpdateChessStatus(bool isCheck, bool hasMoves)
        {
            if (isCheck)
            {
                ChessStatus = hasMoves ? ChessStatus.Check : ChessStatus.Mate;
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


        private static bool CanAvoidCheck(PieceColor color, Location locationFrom)
        {
            foreach (var locationTo in board.GetPiece(locationFrom).GetMoves(locationFrom, board))
            {
                using (board.PerformTemporaryMove(locationFrom, locationTo))
                {
                    if (!IsCheckFor(color))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        // check — это шах
        private static bool IsCheckFor(PieceColor color)
        {
            var piecesFor = color == PieceColor.Black ? PieceColor.White : PieceColor.Black;
            foreach (var loc in board.GetPieces(piecesFor))
            {
                var piece = board.GetPiece(loc);
                var moves = piece.GetMoves(loc, board);
                if (moves
                    .Any(destination
                        => Piece.Is(board.GetPiece(destination), color, PieceType.King)))
                    return true;
            }

            return false;
        }
    }
}
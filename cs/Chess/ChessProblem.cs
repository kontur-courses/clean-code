using System;
using System.Linq;

namespace Chess
{
    public class ChessProblem
    {
        private Board board;
        
        public  ChessProblem(Board board)
        {
            this.board = board;
        }

        // Определяет мат, шах или пат белым.
        public ChessStatus GetChessStatus(PieceColor pieceColor = PieceColor.White)
        {
            var hasMoves = HasMoves(pieceColor);

            if (IsCheck(pieceColor))
                return  hasMoves ? ChessStatus.Check : ChessStatus.Mate;
            return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
        }

        private bool HasMoves(PieceColor pieceColor) =>
            board.GetMovesForColor(PieceColor.White).Any(move => IsMoveSavingKing(move, pieceColor));

        private bool IsMoveSavingKing(Move move, PieceColor pieceColor)
        {
            using (board.PerformTemporaryMove(move.locFrom, move.locTo))
            {
                return !IsCheck(pieceColor);
            }
        }

        // check — это шах
        private bool IsCheck(PieceColor pieceColor) => 
            board.GetMovesForColor(pieceColor.Invert())
                .Any( move => Piece.Is(board[move.locTo], pieceColor, PieceType.King));
        
    }
}
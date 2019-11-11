using System.Collections.Generic;

namespace Chess
{
    public class Piece
    {
        public readonly PieceColor Color;
        public readonly PieceType PieceType;

        public Piece(PieceType pieceType, PieceColor color)
        {
            PieceType = pieceType;
            Color = color;
        }

        public IEnumerable<Location> GetMoves(Location location, Board board)
        {
            return PieceType.GetMoves(location, board);
        }

        public override string ToString()
        {
            var c = PieceType == null ? " ." : " " + PieceType;
            return Color == PieceColor.Black ? c.ToLower() : c;
        }

        public static bool Is(Piece piece, PieceColor color)
        {
            return piece != null && piece.Color == color;
        }

        public static bool Is(Piece piece, PieceColor color, PieceType pieceType)
        {
            return Is(piece, color) && piece.PieceType == pieceType;
        }
    }
}
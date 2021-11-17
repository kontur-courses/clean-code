using System.Collections.Generic;

namespace Chess
{
    public class Piece
    {
        public readonly PieceColor Color;
        private readonly PieceType pieceType;

        public Piece(PieceType pieceType, PieceColor color)
        {
            this.pieceType = pieceType;
            Color = color;
        }

        public IEnumerable<Location> GetMoves(Location location, Board board) => 
            pieceType.GetMoves(location, board);

        public override string ToString()
        {
            var c = pieceType == null 
                ? " ." 
                : $" {pieceType}";
            
            return Color == PieceColor.Black 
                ? c.ToLower() 
                : c;
        }

        public static bool Is(Piece piece, PieceColor color) =>
            piece != null && piece.Color == color;

        public static bool Is(Piece piece, PieceColor color, PieceType pieceType) =>
            Is(piece, color) && piece.pieceType == pieceType;
    }
}
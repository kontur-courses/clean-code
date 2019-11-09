namespace Chess
{
    public enum PieceColor
    {
        Black,
        White
    }

    public static class PieceColorExtension
    {
        public static PieceColor Invert(this PieceColor pieceColor) =>
            pieceColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }
}
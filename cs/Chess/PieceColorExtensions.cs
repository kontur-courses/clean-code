namespace Chess
{
    public static class PieceColorExtensions
    {
        public static PieceColor Opposite(this PieceColor color)
        {
            return color == PieceColor.Black ? PieceColor.White : PieceColor.Black;
        }
    }
}
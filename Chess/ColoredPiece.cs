namespace Chess
{
	public enum PieceColor
	{
		Black,
		White
	}

	public class ColoredPiece
	{
		public static readonly ColoredPiece Empty = new ColoredPiece(null, PieceColor.White);
		public readonly PieceColor Color;
		public readonly Piece Piece;

		public ColoredPiece(Piece piece, PieceColor color)
		{
			Piece = piece;
			Color = color;
		}

		public bool Is(PieceColor color, Piece piece)
		{
			return Piece == piece && Color == color;
		}

		public override string ToString()
		{
			var c = Piece == null ? " ." : " " + Piece.Sign;
			return Color == PieceColor.Black ? c.ToLower() : c;
		}
	}
}
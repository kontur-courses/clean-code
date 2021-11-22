using System;
using System.Linq;

namespace Chess
{
    public class BoardParser
    {
        public Board ParseBoard(string[] lines)
        {
            if (lines.Length != 8) throw new ArgumentException("Should be exactly 8 lines");
            if (lines.Any(line => line.Length != 8)) throw new ArgumentException("All lines should have 8 chars length");

            var cells = new Piece[8][];
            for (var y = 0; y < 8; y++)
            {
                var line = lines[y];
                if (line == null) throw new Exception("incorrect input");
                cells[y] = new Piece[8];
                for (var x = 0; x < 8; x++)
                    cells[y][x] = ParsePiece(line[x]);
            }
            return new Board(cells);
        }

        private static Piece ParsePiece(char pieceSign)
        {
            var color = char.IsUpper(pieceSign) ? PieceColor.White : PieceColor.Black;
            var pieceType = ParsePieceType(char.ToUpper(pieceSign));
            return pieceType == null ? null : new Piece(pieceType, color);
        }

        private static PieceType ParsePieceType(char sign)
        {
            switch (sign)
            {
                case 'R': return PieceType.Rook;
                case 'K': return PieceType.King;
                case 'N': return PieceType.Knight;
                case 'B': return PieceType.Bishop;
                case 'Q': return PieceType.Queen;
                case ' ': 
                case '.': return null;
                default: throw new FormatException("Unknown chess piece " + sign);
            }
        }
    }
}
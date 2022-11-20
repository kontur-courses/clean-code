using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class BoardParser
    {
        private const int MaxLinesLength = 8;
        
        private static Dictionary<char, PieceType> charToPieceTypesConverter;
        public BoardParser()
        {
            charToPieceTypesConverter = new Dictionary<char, PieceType>()
            {
                { 'R', PieceType.Rook },
                { 'K', PieceType.King },
                { 'N', PieceType.Knight },
                { 'B', PieceType.Bishop },
                { 'Q', PieceType.Queen },
                { ' ', null },
                { '.', null },
            };
        }
        public Board ParseBoard(string[] lines)
        {
            if (lines.Length != MaxLinesLength) throw new ArgumentException("Should be exactly 8 lines");
            if (lines.Any(line => line.Length != MaxLinesLength)) throw new ArgumentException("All lines should have 8 chars length");

            var cells = new Piece[MaxLinesLength][];
            for (var y = 0; y < MaxLinesLength; y++)
            {
                var line = lines[y];
                if (line == null) throw new Exception("incorrect input");
                cells[y] = new Piece[MaxLinesLength];
                for (var x = 0; x < MaxLinesLength; x++)
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
            if(!charToPieceTypesConverter.ContainsKey(sign))
                throw new FormatException("Unknown chess piece " + sign);
            return charToPieceTypesConverter[sign];
        }
    }
}
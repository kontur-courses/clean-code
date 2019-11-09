using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class Board
    {
        private readonly Piece[][] cells;

        public Board(Piece[][] cells)
        {
            this.cells = cells;
        }

        public static Board LoadFrom(string[] lines) => new BoardParser().ParseBoard(lines);

        public IEnumerable<Location> GetMoves(Location location) =>
            this[location].GetMoves(location, this);

        public IEnumerable<Move> GetMovesForColor(PieceColor pieceColor) =>
            GetPieces(pieceColor)
                .SelectMany(GetMoves, Move.Create);

        public IEnumerable<Location> GetPieces(PieceColor color) =>
            AllBoard().Where(loc => Piece.Is(this[loc], color));

        public TemporaryPieceMove PerformTemporaryMove(Location from, Location to)
        {
            var old = this[to];
            this[to] = this[from];
            this[from] = null;
            return new TemporaryPieceMove(this, from, to, old);
        }

        private IEnumerable<Location> AllBoard()
        {
            for (var y = 0; y < cells.Length; y++)
            for (var x = 0; x < cells[0].Length; x++)
                yield return new Location(x, y);
        }

        public bool Contains(Location loc) =>
            loc.X >= 0 && loc.X < cells[0].Length &&
            loc.Y >= 0 && loc.Y < cells.Length;


        public Piece this[Location loc]
        {
            get => Contains(loc) ? cells[loc.Y][loc.X] : null;
            set => cells[loc.Y][loc.X] = value;
        }
    }
}
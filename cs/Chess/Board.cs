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

        public IEnumerable<Location> GetPieces(PieceColor color)
        {
            return AllBoard().Where(loc => Piece.Is(GetPiece(loc), color));
        }

        public Piece GetPiece(Location location)
        {
            return Contains(location) ? cells[location.Y][location.X] : null;
        }

        public void Set(Location location, Piece cell)
        {
            cells[location.Y][location.X] = cell;
        }

        public TemporaryPieceMove PerformTemporaryMove(Location from, Location to)
        {
            var old = GetPiece(to);
            Set(to, GetPiece(from));
            Set(from, null);
            return new TemporaryPieceMove(this, from, to, old);
        }

        private IEnumerable<Location> AllBoard()
        {
            for (var y = 0; y < cells.Length; y++)
            for (var x = 0; x < cells[0].Length; x++)
                yield return new Location(x, y);
        }

        public bool Contains(Location loc)
        {
            return loc.X >= 0 && loc.X < cells[0].Length &&
                   loc.Y >= 0 && loc.Y < cells.Length;
        }
    }
}
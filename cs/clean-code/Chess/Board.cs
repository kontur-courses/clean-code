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

        public IEnumerable<Location> GetPieces(PieceColor color) => 
            AllBoard().Where(loc => Piece.Is(GetPiece(loc), color));

        public Piece GetPiece(Location location) => 
            Contains(location) ? cells[location.Y][location.X] : null;

        public void Set(Location location, Piece cell) => 
            cells[location.Y][location.X] = cell;

        public TemporaryPieceMove PerformTemporaryMove(Location from, Location to)
        {
            var old = GetPiece(to);
            Set(to, GetPiece(from));
            Set(from, null);
            return new TemporaryPieceMove(this, from, to, old);
        }

        private IEnumerable<Location> AllBoard()
        {
            for (int y = 0; y < cells.Length; y++)
            for (int x = 0; x < cells[0].Length; x++)
                yield return new Location(x, y);
        }

        public bool Contains(Location loc) =>
            loc.X >= 0 && loc.X < cells[0].Length && 
            loc.Y >= 0 && loc.Y < cells.Length;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class Board
	{
		private readonly Piece[,] cells;

	    public Board(Piece[,] cells)
	    {
	        this.cells = cells;
	    }

        public IEnumerable<Location> GetPieces(PieceColor color) => 
            AllBoard().Where(loc => Piece.Is(GetPiece(loc), color));

        public Piece GetPiece(Location location) => 
            Contains(location) ? cells[location.X, location.Y] : null;

        public void Set(Location location, Piece cell) => 
            cells[location.X, location.Y] = cell;

        public TemporaryPieceMove PerformTemporaryMove(Location from, Location to)
		{
			var old = GetPiece(to);
			Set(to, GetPiece(from));
			Set(from, null);
			return new TemporaryPieceMove(this, from, to, old);
		}

        private IEnumerable<Location> AllBoard()
        {
            return 
                from y in Enumerable.Range(0, cells.GetLength(0))
                from x in Enumerable.Range(0, cells.GetLength(1))
                select new Location(x, y);
        }

        public bool Contains(Location loc) =>
            loc.X >= 0 && loc.X < cells.GetLength(0) && 
            loc.Y >= 0 && loc.Y < cells.GetLength(1);
	}
}
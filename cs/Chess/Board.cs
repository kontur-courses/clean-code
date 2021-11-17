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

        public void SetPieceToLocation(Location location, Piece cell) => 
            cells[location.Y][location.X] = cell;

        public TemporaryPieceMove PerformTemporaryMove(Location departure, Location destination)
        {
            var oldPiece = GetPiece(destination);
            SetPieceToLocation(destination, GetPiece(departure));
            SetPieceToLocation(departure, null);
            
            return new TemporaryPieceMove(this, departure, destination, oldPiece);
        }

        private IEnumerable<Location> AllBoard()
        {
            for (var y = 0; y < cells.Length; y++)
            for (var x = 0; x < cells[0].Length; x++)
                yield return new Location(x, y);
        }

        public bool Contains(Location loc) =>
            loc.X >= 0 
            && loc.X < cells[0].Length 
            && loc.Y >= 0 
            && loc.Y < cells.Length;
    }
}
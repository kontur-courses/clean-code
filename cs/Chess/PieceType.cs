using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class PieceType
    {
        public static readonly PieceType Rook = 
            new PieceType(true, 'R', new Location(1, 0), new Location(0, 1));
        public static readonly PieceType King = 
            new PieceType(false, 'K', new Location(1, 1), new Location(1, 0), new Location(0, 1));
        public static readonly PieceType Queen = 
            new PieceType(true, 'Q', new Location(1, 1), new Location(1, 0), new Location(0, 1));
        public static readonly PieceType Bishop = 
            new PieceType(true, 'B', new Location(1, 1));
        public static readonly PieceType Knight = 
            new PieceType(false, 'N', new Location(2, 1), new Location(1, 2));

        private readonly Location[] directions;
        private readonly bool infinity;
        private readonly char sign;

        private PieceType(bool infinity, char sign, params Location[] directions)
        {
            this.infinity = infinity;
            this.sign = sign;
            this.directions = directions
                .Union(directions.Select(d => new Location(-d.X, d.Y)))
                .Union(directions.Select(d => new Location(d.X, -d.Y)))
                .Union(directions.Select(d => new Location(-d.X, -d.Y)))
                .ToArray();
        }

        public override string ToString() => sign.ToString();

        public IEnumerable<Location> GetMoves(Location location, Board board) => 
            directions.SelectMany(d => MovesInOneDirection(location, board, d, infinity));

        private static IEnumerable<Location> MovesInOneDirection(Location from, Board board, 
            Location destination, bool infinity)
        {
            var piece = board.GetPiece(from);
            var distance = infinity ? int.MaxValue : 1;
            
            for (var i = 0; i < distance; i++)
            {
                var newLocation = new Location(from.X + destination.X * (i + 1), from.Y + destination.Y * (i + 1));
                
                if (!board.Contains(newLocation)) break;
                var destinationPiece = board.GetPiece(newLocation);
                
                if (destinationPiece == null)
                {
                    yield return newLocation;
                }
                else
                {
                    if (destinationPiece.Color != piece.Color)
                    {
                        yield return newLocation;
                    }
                    
                    yield break;
                }
            }
        }
    }
}
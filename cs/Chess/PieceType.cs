using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class PieceType
    {
        public static readonly PieceType Rook = new PieceType(
            true, 'R', new Location(1, 0), new Location(0, 1));
        public static readonly PieceType King = new PieceType(
            false, 'K', new Location(1, 1), new Location(1, 0), new Location(0, 1));
        public static readonly PieceType Queen = new PieceType(
            true, 'Q', new Location(1, 1), new Location(1, 0), new Location(0, 1));
        public static readonly PieceType Bishop = new PieceType(
            true, 'B', new Location(1, 1));
        public static readonly PieceType Knight = new PieceType(
            false, 'N', new Location(2, 1), new Location(1, 2));

        private readonly Location[] directions;
        private readonly bool isInfinite;
        private readonly char sign;

        private PieceType(bool isInfinite, char sign, params Location[] directions)
        {
            this.isInfinite = isInfinite;
            this.sign = sign;
            this.directions = AddInvertedSteps(directions);
        }

        private Location[] AddInvertedSteps(Location[] directions)
        {
            return directions
                .Union(directions.Select(d => new Location(-d.X, d.Y)))
                .Union(directions.Select(d => new Location(d.X, -d.Y)))
                .Union(directions.Select(d => new Location(-d.X, -d.Y)))
                .ToArray();
        }

        public override string ToString() => sign.ToString();

        public IEnumerable<Location> GetMoves(Location location, Board board) => 
            directions.SelectMany(d => GetPossibleLocationsToDirection(location, board, d, isInfinite));

        private static IEnumerable<Location> GetPossibleLocationsToDirection(
            Location startLocation, Board board, Location direction, bool isInfinite)
        {
            var piece = board.GetPiece(startLocation);
            var distance = isInfinite ? int.MaxValue : 2;
            for (var i = 1; i < distance; i++)
            {
                var nextLocation = new Location(startLocation.X + direction.X*i, startLocation.Y + direction.Y*i);
                if (!board.Contains(nextLocation))
                    break;
                var destinationPiece = board.GetPiece(nextLocation);
                if (destinationPiece == null)
                    yield return nextLocation;
                else
                {
                    if (piece.IsOpponentPiece(destinationPiece))
                        yield return nextLocation;
                    yield break;
                }
            }
        }
    }
}
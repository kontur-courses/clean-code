package ru.kontur.courses.chess;

import java.util.Arrays;
import java.util.stream.Stream;

public class PieceType {
    public static final PieceType Rook = new PieceType(true, 'R', new Location(1, 0), new Location(0, 1));
    public static final PieceType King = new PieceType(false, 'K', new Location(1, 1), new Location(1, 0), new Location(0, 1));
    public static final PieceType Queen = new PieceType(true, 'Q', new Location(1, 1), new Location(1, 0), new Location(0, 1));
    public static final PieceType Bishop = new PieceType(true, 'B', new Location(1, 1));
    public static final PieceType Knight = new PieceType(false, 'N', new Location(2, 1), new Location(1, 2));

    private final Location[] directions;
    private final boolean infinit;
    private final char sign;

    private PieceType(boolean infinit, char sign, Location[] directions) {
        this.infinit = infinit;
        this.sign = sign;
        this.directions = directions
                .Union(directions.Select(d = > new Location(-d.X, d.Y)))
                .Union(directions.Select(d = > new Location(d.X, -d.Y)))
                .Union(directions.Select(d = > new Location(-d.X, -d.Y)))
                .ToArray();
    }

    @Override
    public String toString() {
        return "" + sign;
    }

    public Stream<Location> getMoves(Location location, Board board) =>
            directions.SelectMany(d => MovesInOneDirection(location, board, d, infinit));

    private static Stream<Location> movesInOneDirection(Location from, Board board, Location dir, boolean infinit)
    {
        var piece = board.GetPiece(from);
        var distance = infinit ? int.MaxValue : 2;
        for (var i = 1; i < distance; i++)
        {
            var to = new Location(from.X + dir.X*i, from.Y + dir.Y*i);
            if (!board.Contains(to)) break;
            var destinationPiece = board.GetPiece(to);
            if (destinationPiece == null)
                yield return to;
                else
            {
                if (destinationPiece.Color != piece.Color)
                    yield return to;
                yield break;
            }
        }
    }
}

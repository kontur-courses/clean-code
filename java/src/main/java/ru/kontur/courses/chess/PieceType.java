package ru.kontur.courses.chess;

import java.util.*;
import java.util.stream.Stream;

public class PieceType {
    public static final PieceType rook = new PieceType(true, 'R', new Location[]{new Location(1, 0), new Location(0, 1)});
    public static final PieceType king = new PieceType(false, 'K', new Location[]{new Location(1, 1), new Location(1, 0), new Location(0, 1)});
    public static final PieceType queen = new PieceType(true, 'Q', new Location[]{new Location(1, 1), new Location(1, 0), new Location(0, 1)});
    public static final PieceType bishop = new PieceType(true, 'B', new Location[]{new Location(1, 1)});
    public static final PieceType knight = new PieceType(false, 'N', new Location[]{new Location(2, 1), new Location(1, 2)});

    private final Location[] directions;
    private final boolean infinit;
    private final char sign;

    private PieceType(boolean infinit, char sign, Location[] directions) {
        this.infinit = infinit;
        this.sign = sign;
        this.directions = union(
                union(
                        union(directions, Arrays.stream(directions).map(it -> new Location(-it.x(), it.y())).toList()),
                        Arrays.stream(directions).map(it -> new Location(it.x(), -it.y())).toList()
                ),
                Arrays.stream(directions).map(it -> new Location(-it.x(), -it.y())).toList()
        );
    }

    private Location[] union(Location[] directions, List<Location> others) {
        var hashSet = new HashSet<>(Arrays.stream(directions).toList());
        hashSet.addAll(others);

        return hashSet.toArray(Location[]::new);
    }

    @Override
    public String toString() {
        return "" + sign;
    }

    public Stream<Location> getMoves(Location location, Board board) {
        return Arrays.stream(directions).flatMap(it -> movesInOneDirection(location, board, it, infinit));
    }

    private static Stream<Location> movesInOneDirection(Location from, Board board, Location dir, Boolean infinit) {
        var piece = board.getPiece(from);
        var distance = infinit ? Integer.MAX_VALUE : 2;
        var result = new ArrayList<Location>();
        for (var i = 1; i < distance; i++) {
            var to = new Location(from.x() + dir.x() * i, from.y() + dir.y() * i);
            if (!board.contains(to)) break;
            var destinationPiece = board.getPiece(to);
            if (destinationPiece == null)
                result.add(to);
            else {
                if (destinationPiece.color != piece.color)
                    result.add(to);
                break;
            }
        }

        return result.stream();
    }
}

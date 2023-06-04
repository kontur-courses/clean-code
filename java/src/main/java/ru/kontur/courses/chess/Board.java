package ru.kontur.courses.chess;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.function.Supplier;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Board {
    private final Piece[][] cells;

    public Board(Piece[][] cells) {
        this.cells = cells;
    }

    public Iterable<Location> getPieces(PieceColor color) {
        return allBoards().filter(it -> Piece.is(getPiece(it), color)).collect(Collectors.toList());
    }

    public Piece getPiece(Location location) {
        return contains(location) ? cells[location.y()][location.x()] : null;
    }

    public void set(Location location, Piece cell) {
        cells[location.y()][location.x()] = cell;
    }

    public TemporaryPieceMove performTemporaryMove(Location from, Location to) {
        var old = getPiece(to);
        set(to, getPiece(from));
        set(from, null);
        return new TemporaryPieceMove(this, from, to, old);
    }

    public Stream<Location> allBoards() {
        var result = new ArrayList<Location>();
        for (int y = 0; y < cells.length; y++)
            for (int x = 0; x < cells[0].length; x++) {
                result.add(new Location(x, y));
            }

        return result.stream();
    }

    public boolean contains(Location loc) {
        return loc.x() >= 0 && loc.x() < cells[0].length &&
                loc.y() >= 0 && loc.y() < cells.length;
    }
}

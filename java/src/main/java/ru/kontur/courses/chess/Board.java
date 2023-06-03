package ru.kontur.courses.chess;

import java.util.ArrayList;
import java.util.function.Supplier;
import java.util.stream.Stream;

public class Board {
    private final Piece[][] cells;

    public Board(Piece[][] cells) {
        this.cells = cells;
    }

    public Stream<Location> getPieces(PieceColor color) {
        contains()
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
        return loc.x >= 0 && loc.y < cells[0].length &&
                loc.y >= 0 && loc.y < cells.length;
    }
}

package ru.kontur.courses.chess;

import java.io.Closeable;
import java.io.IOException;

public class TemporaryPieceMove implements Closeable {
    private final Board board;
    private final Location from;
    private final Piece oldDestinationPiece;
    private final Location to;

    public TemporaryPieceMove(Board board, Location from, Location to, Piece oldDestinationPiece) {
        this.board = board;
        this.from = from;
        this.to = to;
        this.oldDestinationPiece = oldDestinationPiece;
    }

    public void undo() {
        board.set()
    }

    @Override
    public void close() throws IOException {
        undo();
    }
}

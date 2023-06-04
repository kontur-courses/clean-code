package ru.kontur.courses.chess;

import java.util.stream.Collectors;

public class Piece {
    public final PieceColor color;
    public final PieceType pieceType;

    public Piece(PieceType pieceType, PieceColor color) {
        this.pieceType = pieceType;
        this.color = color;
    }

    public Iterable<Location> getMoves(Location location, Board board) {
        return pieceType.getMoves(location, board).collect(Collectors.toList());
    }

    @Override
    public String toString() {
        var c = pieceType == null ? " ." : " " + pieceType;
        return color == PieceColor.BLACK ? c.toLowerCase() : c;
    }

    public static boolean is(Piece piece, PieceColor color) {
        return piece != null && piece.color == color;
    }

    public static boolean is(Piece piece, PieceColor color, PieceType pieceType) {
        return is(piece, color) && piece.pieceType == pieceType;
    }
}

package ru.kontur.courses.chess;

import java.util.Arrays;

public class BoardParser {

    public Board parseBoard(String[] lines) throws Exception {
        if (lines.length != 8) throw new IllegalArgumentException("Should be exactly 8 lines");
        if (Arrays.stream(lines).anyMatch(it -> it.length() != 8))
            throw new IllegalArgumentException("All lines should have 8 chars length");

        var cells = new Piece[8][];
        for (var y = 0; y < 8; y++) {
            var line = lines[y];
            if (line == null) throw new Exception("incorrect input");
            cells[y] = new Piece[8];
            for (var x = 0; x < 8; x++)
                cells[y][x] = parsePiece(line.charAt(x));
        }
        return new Board(cells);
    }

    private static Piece parsePiece(char pieceSign) {
        var color = Character.isUpperCase(pieceSign) ? PieceColor.WHITE : PieceColor.BLACK;
        var pieceType = parsePieceType(Character.toUpperCase(pieceSign));
        return pieceType == null ? null : new Piece(pieceType, color);
    }

    private static PieceType parsePieceType(char sign) {
        switch (sign) {
            case 'R':
                return PieceType.rook;
            case 'K':
                return PieceType.king;
            case 'N':
                return PieceType.knight;
            case 'B':
                return PieceType.bishop;
            case 'Q':
                return PieceType.queen;
            case ' ':
            case '.':
                return null;
            default:
                throw new IllegalArgumentException("Unknown chess piece " + sign);
        }
    }
}

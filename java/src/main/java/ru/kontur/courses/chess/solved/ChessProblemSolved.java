package ru.kontur.courses.chess.solved;

import ru.kontur.courses.chess.*;

import java.util.stream.Stream;
import java.util.stream.StreamSupport;

public class ChessProblemSolved {
    private final Board board;

    public ChessProblemSolved(Board board) {
        this.board = board;
    }

    public ChessStatus getStatusFor(PieceColor color) {
        var isCheck = isCheckFor(color);
        var hasMoves = hasSafeMovesFor(color);
        if (isCheck)
            return hasMoves ? ChessStatus.CHECK : ChessStatus.MATE;
        else
            return hasMoves ? ChessStatus.OK : ChessStatus.STALEMATE;
    }

    private boolean isCheckFor(PieceColor color) {
        PieceColor invert = null;
        switch (color) {
            case BLACK -> {
                invert = PieceColor.WHITE;
                break;
            }
            case WHITE -> {
                invert = PieceColor.BLACK;
                break;
            }
        }

        return getAllMovesOf(invert).anyMatch(it -> Piece.is(board.getPiece(it.to()), color, PieceType.king));
    }

    private boolean hasSafeMovesFor(PieceColor color) {
        return getAllMovesOf(color).anyMatch(this::isSafeMove);
    }

    private Stream<ChessMove> getAllMovesOf(PieceColor color) {
        return StreamSupport.stream(board.getPieces(color).spliterator(), false).flatMap(this::getMoves);
    }


    private Stream<ChessMove> getMoves(Location pieceLoc) {
        return StreamSupport.stream(board.getPiece(pieceLoc).getMoves(pieceLoc, board).spliterator(), false).map(
                it -> new ChessMove(pieceLoc, it)
        );
    }


    private boolean isSafeMove(ChessMove move) {
        var pieceColor = board.getPiece(move.from()).color;
        try (var ignored = board.performTemporaryMove(move.from(), move.to())) {
            return !isCheckFor(pieceColor);
        }
    }
}
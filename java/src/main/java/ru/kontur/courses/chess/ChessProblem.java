package ru.kontur.courses.chess;

public class ChessProblem {
    private static Board board;
    public static ChessStatus chessStatus;

    public static void loadFrom(String[] lines) throws Exception {
        board = new BoardParser().parseBoard(lines);
    }

    // Определяет мат, шах или пат белым.
    public static void calculateChessStatus() {
        var isCheck = isCheckForWhite();
        var hasMoves = false;
        for (var locFrom : board.getPieces(PieceColor.WHITE)) {
            for (var locTo : board.getPiece(locFrom).getMoves(locFrom, board)) {
                var old = board.getPiece(locTo);
                board.set(locTo, board.getPiece(locFrom));
                board.set(locFrom, null);
                if (!isCheckForWhite())
                    hasMoves = true;
                board.set(locFrom, board.getPiece(locTo));
                board.set(locTo, old);
            }
        }
        if (isCheck)
            if (hasMoves)
                chessStatus = ChessStatus.CHECK;
            else chessStatus = ChessStatus.MATE;
        else if (hasMoves) chessStatus = ChessStatus.OK;
        else chessStatus = ChessStatus.STALEMATE;
    }

    // check — это шах
    private static boolean isCheckForWhite() {
        var isCheck = false;
        for (var loc : board.getPieces(PieceColor.BLACK)) {
            var piece = board.getPiece(loc);
            var moves = piece.getMoves(loc, board);
            for (var destination : moves) {
                if (Piece.is(board.getPiece(destination), PieceColor.WHITE, PieceType.king))
                    isCheck = true;
            }
        }
        if (isCheck) return true;
        return false;
    }
}

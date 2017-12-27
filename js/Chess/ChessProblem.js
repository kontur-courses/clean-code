import BoardParser from './BoardParser'
import PieceColor from './PieceColor'
import PieceType from './PieceType'
import ChessStatus from './ChessStatus'

export class ChessProblem {
    static board
    static chessStatus

    static loadFrom(lines) {
        this.board = new BoardParser().parseBoard(lines);
    }

    // Определяет мат, шах или пат белым.
    static calculateChessStatus() {
        const isCheck = ChessProblem.isCheckForWhite();
        let hasMoves = false;

        for (let locFrom of this.board.getPieces(PieceColor.white)) {
            for (let locTo of this.board.getPiece(locFrom).getMoves(locFrom, this.board)) {
                const old = this.board.getPiece(locFrom);
                this.board.set(locTo, this.board.getPiece(locFrom))
                this.board.set(locFrom, null);
                if (!ChessProblem.isCheckForWhite())
                    hasMoves = true;
                this.board.set(locFrom, this.board.getPiece(locTo));
                this.board.set(locTo, old);
            }
        }

        if (isCheck)
            if (hasMoves)
                this.chessStatus = ChessStatus.check;
            else this.chessStatus = ChessStatus.mate;
        else if (hasMoves) this.chessStatus = ChessStatus.ok;
        else this.chessStatus = ChessStatus.stalemate;
    }

    // check — это шах
    static isCheckForWhite() {

        let isCheck = false;
        for (let loc of this.board.getPieces(PieceColor.black)) {
            const piece = this.board.getPiece(loc);
            const moves = piece.getMoves(loc, this.board);

            for (let destination of moves) {
                const destinationPiece = this.board.getPiece(destination)
                if (destinationPiece && destinationPiece.is(PieceColor.white, PieceType.King))
                    isCheck = true;
            }
        }
        if (isCheck === true) return true;
        return false;
    }
}

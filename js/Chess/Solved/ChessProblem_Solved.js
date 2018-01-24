import PieceColor from '../PieceColor'
import PieceType from '../PieceType'
import ChessStatus from '../ChessStatus'

export default class ChessProblem {
    static board

    constructor(board) {
        this.board = board;
    }

    getStatusFor(color) {
        const isCheck = this.isCheckFor(color);
        const hasMoves = this.hasMovesFor(color, this.isValidMove);
        if (isCheck){
            return hasMoves ? ChessStatus.check : ChessStatus.mate;
        } else {
            return hasMoves ? ChessStatus.ok : ChessStatus.stalemate;
        }
    }

    isCheckFor(color) {
        const invertedColor = color === PieceColor.white ? PieceColor.black : PieceColor.white;
        return this.hasMovesFor(invertedColor, (from, to) => {
            const piece = this.board.getPiece(to)
            if (piece) {
                return piece.is(color, PieceType.King)
            }
            return false
        })
    }

    hasMovesFor(color, isAcceptableMove) {
        const moves = this.board.getPieces(color).filter(pieceLoc =>
            this.getMoves(pieceLoc).filter(destLoc => isAcceptableMove(pieceLoc, destLoc)).length
        )

        return Boolean(moves.length);
    }

    getMoves(pieceLoc) {
        return this.board.getPiece(pieceLoc).getMoves(pieceLoc, this.board)
    }

    isValidMove = (from, to) => {
        const pieceColor = this.board.getPiece(from).color;
        const move = this.board.performTemporaryMove(from, to);
        const isValid = !this.isCheckFor(pieceColor);
        move.undo();
        return isValid;
    }
}

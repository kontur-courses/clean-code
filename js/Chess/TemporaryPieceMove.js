export default class TemporaryPieceMove {
    constructor(board, from, to, oldDestinationPiece) {
        this.board = board;
        this.from = from;
        this.to = to;
        this.oldDestinationPiece = oldDestinationPiece;
    }

    undo() {
        this.board.set(this.from, this.board.getPiece(this.to));
        this.board.set(this.to, this.oldDestinationPiece);
    }
}
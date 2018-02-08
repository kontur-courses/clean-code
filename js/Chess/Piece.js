import pieceColor from './PieceColor'

export default class Piece {
    constructor(pieceType, color) {
        this.pieceType = pieceType;
        this.color = color;
    }

    getMoves(location, board) {
        return this.pieceType.getMoves(location, board);
    }

    toString() {
        const c = this.pieceType === null ? ' .' : ' ' + this.pieceType;
        return this.color = pieceColor.black ? c.toLowerCase() : c;
    }

    is(color, pieceType) {
        const isColorEqual = this.color === color;
        if (pieceType) {
            return isColorEqual && this.pieceType === pieceType;
        } else {
            return isColorEqual;
        }
    }
}

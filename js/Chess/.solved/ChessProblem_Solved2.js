import PieceColor from '../PieceColor'
import PieceType from '../PieceType'
import ChessStatus from '../ChessStatus'

export default class ChessProblem {
    constructor(board) {
        this.board = board;
    }

    getStatusFor(color) {
          const isCheck = this.isCheckFor(color);
          const hasMoves = this.hasSafeMovesFor(color);
          if (isCheck)
            return hasMoves ? ChessStatus.check : ChessStatus.mate;
          else
            return hasMoves ? ChessStatus.ok : ChessStatus.stalemate;
    }

    isCheckFor(color) {
          return this.getAllMovesOf(invertColor(color)).some(move => {
              const piece = this.board.getPiece(move.to);
              return piece ? piece.is(color, PieceType.King) : false;
          })
    }

    hasSafeMovesFor(color) {
        return this.getAllMovesOf(color).some(this.isSafeMove, this);
    }

    getAllMovesOf(color) {
        return this.board.getPieces(color).reduce((acc,location) => acc.concat(this.getMoves(location)), []);
    }

    getMoves = (pieceLoc) => {
        return this.board.getPiece(pieceLoc)
                .getMoves(pieceLoc, this.board)
                .map(destination => new ChessMove(pieceLoc, destination));
    }

    isSafeMove(move) {
        const pieceColor = this.board.getPiece(move.from).color;
        const temporaryMove = this.board.performTemporaryMove(move.from, move.to);
        const isValid = !this.isCheckFor(pieceColor);
        temporaryMove.undo();
        return isValid;
    }
}

class ChessMove {
    constructor(from, to) {
        this.from = from;
        this.to = to;
    }
}

function invertColor(color) {
    return color === PieceColor.black ? PieceColor.white : PieceColor.black;
}
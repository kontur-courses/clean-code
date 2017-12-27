class Board {
    constructor(cells) {
        this.cells = cells;
    }

    getPieces(color) {
        return this.allBoard().filter(loc => this.getPiece(loc) && this.getPiece(loc).color === color);
    }

    getPiece(location) {
        return this.contains(location) ? this.cells[location.y][location.x] : null;
    }

    set(location, cell) {
        return this.cells[location.y][location.x] = cell;
    }

    performTemporaryMove(from, to) {
        const old = this.getPiece(to);
        this.set(to, this.getPiece(from));
        this.set(from, null)
        return new TemporaryPieceMove(this, from, to, old)
    }

    allBoard() {
        return this.cells.reduce((allLocations, row, x) =>
            allLocations.concat(row.reduce((rowLocations, _, y) =>
                rowLocations.concat({x, y}), [])),
            [])
    }

    contains(location) {
        return location.x >= 0 && location.x < this.cells[0].length &&
            location.y >= 0 && location.y < this.cells.length;
    }
}

class TemporaryPieceMove {
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

    dispose() {
        this.undo();
    }
}

module.exports = Board;
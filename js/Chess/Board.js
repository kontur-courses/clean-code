import Location from './Location'
import TemporaryPieceMove from './TemporaryPieceMove'

export default class Board {
    constructor(cells) {
        this.cells = cells;
    }

    getPieces(color) {
        return this.allBoard().filter(loc => this.getPiece(loc) && this.getPiece(loc).color === color);
    }

    getPiece(location) {
        return this.contains(location) ? this.cells[location.x][location.y] : null;
    }

    set(location, cell) {
        this.cells[location.x][location.y] = cell;
    }

    performTemporaryMove(from, to) {
        const old = this.getPiece(to);
        this.set(to, this.getPiece(from));
        this.set(from, null);
        return new TemporaryPieceMove(this, from, to, old);
    }

    allBoard() {
        const result = [];

        for (let y = 0; y < this.cells.length; y++) {
            for (let x = 0; x < this.cells[0].length; x++) {
                result.push(new Location(x, y));
            }
        }

        return result;
    }

    contains(location) {
        return location.x >= 0 && location.x < this.cells[0].length &&
            location.y >= 0 && location.y < this.cells.length;
    }
}

import Location from './Location'

export default class PieceType {
    static Rook = new PieceType(true, 'R', new Location(1, 0), new Location(0, 1));
    static King = new PieceType(false, 'K', new Location(1, 1), new Location(1, 0), new Location(0, 1));
    static Queen = new PieceType(true, 'Q', new Location(1, 1), new Location(1, 0), new Location(0, 1));
    static Bishop = new PieceType(true, 'B', new Location(1, 1));
    static Knight = new PieceType(false, 'N', new Location(2, 1), new Location(1, 2));

    constructor(infinite, sign, ...directions) {
        this.infinite = infinite;
        this.sign = sign;
        this.directions = getPermutations(directions);
    }

    getMoves(from, board) {
        return this.directions.reduce((res, direction) => res
            .concat(this.movesInOneDirection(from, board, direction)),
        []);
    }

    movesInOneDirection(from, board, direction) {
        const piece = board.getPiece(from);
        const result = [];
        let distance = 1;

        do {
            const to = sum(from, multiply(direction, distance));
            if (!board.contains(to)) {
                break;
            }
            const destinationPiece = board.getPiece(to);
            if (destinationPiece === null) {
                result.push(to);
            } else {
                if (destinationPiece.color !== piece.color) {
                    result.push(to);
                }
                break;
            }
        } while (this.infinite && distance++);

        return result;
    }

    toString() {
        return this.sign;
    }
}

function equals(a, b) {
    return a.x === b.x && a.y === b.y;
}

function multiply(a, b) {
    if (typeof b === 'number') {
        return new Location(a.x * b, a.y * b);
    }
    return new Location(a.x * b.x, a.y * b.y);
}

function sum(a, b) {
    return new Location(a.x + b.x, a.y + b.y);
}

function contains(arr, val) {
    return arr.some(item => equals(val, item));
}

function getPermutations(directions) {
    const deltas = [
        {x: 1, y: 1},
        {x: -1, y: 1},
        {x: 1, y: -1},
        {x: -1, y: -1},
    ];

    const permutations = [];
    for (const direction of directions) {
        for (const delta of deltas) {
            const current = multiply(direction, delta);
            if (!contains(permutations, current)) {
                permutations.push(current);
            }
        }
    }
    return permutations;
}
import pieceColor from './PieceColor'
import Piece from './Piece'
import PieceType from './PieceType'
import Board from './Board'

export default class BoardParser {
    parseBoard(lines) {
        if (lines.length !== 8) {
            throw new Error('Should be exactly 8 lines');
        }

        if (lines.some(line => line.length !== 8)) {
            throw new Error('All lines should have 8 chars length');
        }

        const cells = [];
        for (let y = 0; y < 8; y++) {
            const line = lines[y];
            if (!line) {
                throw new Error('incorrect input');
            }
            cells[y] = [];
            for (let x = 0; x < 8; x++) {
                cells[y][x] = this.parsePiece(line[x]);
            }
        }
        return new Board(cells);
    }

    parsePiece(pieceSign) {
        const color = /[A-Z]/.test(pieceSign) ? pieceColor.white : pieceColor.black;
        const pieceType = this.parsePieceType(pieceSign.toUpperCase());
        return pieceType === null ? null : new Piece(pieceType, color);
    }

    parsePieceType(sign) {
        switch (sign) {
            case 'R': return PieceType.Rook;
            case 'K': return PieceType.King;
            case 'N': return PieceType.Knight;
            case 'B': return PieceType.Bishop;
            case 'Q': return PieceType.Queen;
            case ' ':
            case '.': return null;
            default: throw new Error("Unknown chess piece " + sign);
        }
    }
}

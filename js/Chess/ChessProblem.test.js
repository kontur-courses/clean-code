import { ChessProblem } from './ChessProblem'
import ChessStatus from './ChessStatus'

describe('ChessProblem', () => {
    test('tests', () => {
        for (let i = 0; i < 2; i++) {
            const board = getTest(i);
            ChessProblem.loadFrom(board);
            const expectedAnswer = getAnswer(i);
            ChessProblem.calculateChessStatus();
            expect(ChessProblem.chessStatus).toBe(expectedAnswer);
        }
    });

    test('RepeatedMethodCallDoNotChangeBehaviour', () => {
        const board = [
            "        ",
            "        ",
            "        ",
            "   q    ",
            "    K   ",
            " Q      ",
            "        ",
            "        ",
        ];
        ChessProblem.loadFrom(board);
        ChessProblem.calculateChessStatus();
        expect(ChessProblem.chessStatus).toBe(ChessStatus.check);

        // Now check that internal board modifications during the first call do not change answer
        ChessProblem.calculateChessStatus();
        expect(ChessProblem.chessStatus).toBe(ChessStatus.check);
    });
})

function getTest(index) {
    const testBoards = [[
        'Krk.....',
        'R.n.....',
        'qnb.....',
        '........',
        '........',
        '........',
        '........',
        '........',
    ], [
        '........',
        '........',
        '........',
        '........',
        '........',
        'knb.....',
        'r.n.....',
        'KRq.....',
    ]];

    return testBoards[index]
}

function getAnswer(index) {
    const answers = [
        'mate', 'mate',
    ]

    return answers[index]
}
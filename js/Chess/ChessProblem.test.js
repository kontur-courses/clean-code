import ChessProblem from './ChessProblem'
import ChessStatus from './ChessStatus'
import fs from 'fs'

describe('ChessProblem', () => {
    test('repeated method call do not change behaviour', () => {
        const boardLines = [
            "        ",
            "        ",
            "        ",
            "   q    ",
            "    K   ",
            " Q      ",
            "        ",
            "        ",
        ];

        testChess(true, ChessStatus.check, boardLines);

        // Now check that internal board modifications during the first call do not change answer
        testChess(false, ChessStatus.check);
    });

    test('all tests', () => {
        const dirPath = './Chess/ChessTests';
        const readFile = (path) => fs.readFileSync(path).toString().trim().split('\n').map(line => line.trim());
        const allFilesNames = fs.readdirSync(dirPath);
        const inputFiles = allFilesNames.filter(fileName => fileName.endsWith('.in'));
        const answerFiles = allFilesNames.filter(fileName => fileName.endsWith('.ans'));

        for (let i = 0; i < inputFiles.length; i++) {
            const boardLines = readFile(dirPath + '/' + inputFiles[i]);
            const expectedAnswer = fs.readFileSync(dirPath + '/' + answerFiles[i]).toString().trim();
            testChess(true, expectedAnswer, boardLines);
        }
    });
});

function testChess (needLoad, expectedAnswer, boardLines) {
    if (needLoad) {
        ChessProblem.loadFrom(boardLines);
    }
    ChessProblem.calculateChessStatus();
    expect(ChessProblem.chessStatus).toBe(expectedAnswer);
}

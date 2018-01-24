import { ChessProblem } from './ChessProblem'
import ChessStatus from './ChessStatus'
import fs from 'fs'

describe('ChessProblem', () => {
    test('tests', () => {
        const dirPath = './Chess/ChessTests';
        const readFile = (path) => fs.readFileSync(path).toString().trim().split('\r\n');
        const allFilesNames = fs.readdirSync(dirPath);
        const inputFiles = allFilesNames.filter(fileName => fileName.endsWith('.in'));
        const answerFiles = allFilesNames.filter(fileName => fileName.endsWith('.ans'));

        for (let i = 0; i < inputFiles.length; i++) {
            const board = readFile(dirPath + '/' + inputFiles[i]);
            ChessProblem.loadFrom(board);
            const expectedAnswer = fs.readFileSync(dirPath + '/' + answerFiles[i]).toString().trim();
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

import ChessProblem from './ChessProblem_Solved2'
import BoardParser from '../BoardParser'
import PieceColor from '../PieceColor'
import fs from 'fs'

xdescribe('ChessProblem', () => {
    test('tests', () => {
        const dirPath = './Chess/ChessTests';
        const readFile = (path) => fs.readFileSync(path).toString().trim().split('\n');
        const allFilesNames = fs.readdirSync(dirPath);
        const inputFiles = allFilesNames.filter(fileName => fileName.endsWith('.in'));
        const answerFiles = allFilesNames.filter(fileName => fileName.endsWith('.ans'));

        for (let i = 0; i < inputFiles.length; i++) {
            const boardLines = readFile(dirPath + '/' + inputFiles[i]);
            const board = new BoardParser().parseBoard(boardLines);
            const expectedAnswer = fs.readFileSync(dirPath + '/' + answerFiles[i]).toString().trim();
            const whiteStatus = new ChessProblem(board).getStatusFor(PieceColor.white)

            expect(whiteStatus).toBe(expectedAnswer);
        }
    });
})

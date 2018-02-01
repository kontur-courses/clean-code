class Field {
    constructor(width, height, filledCellsLineByLine, score = 0) {
        this.width = width;
        this.height = height;
        this.score = score;
        this.filledCellsLineByLine = filledCellsLineByLine;
    }

    clearFullLines() {
        const notFullLines = this.getAllNotFullLines();
        const clearedLinesCount = this.height - notFullLines.length;
        const newLinesArray = this.createNewLinesArray(clearedLinesCount, notFullLines);
        return new Field(this.width, this.height, newLinesArray, this.score + clearedLinesCount);
    }

    createNewLinesArray(emptyLinesCount, nonEmptyLines) {
        const emptyLines = Array.from(Array(emptyLinesCount));
        return emptyLines.concat(nonEmptyLines);
    }

    getAllNotFullLines()
    {
        return this.filledCellsLineByLine.filter(line =>
            line.filter(item => Boolean(item)).length !== this.width);
    }
}

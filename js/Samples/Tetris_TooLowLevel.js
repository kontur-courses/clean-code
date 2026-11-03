class Tetris_TooLowLevel {
    constructor(width, height, isFilled) {
        this.width = width;
        this.height = height;
        this.isFilled = isFilled;
    }

    clearFullLines() {
        for (let y = 0; y < this.height; y++) {
            let count = 0;
            let fullY = 0;

            for (let x = 0; x < this.width; x++) {
                if (this.isFilled[x][y]) {
                    count++;
                    if (count === this.width) {
                        fullY = y;
                    }
                }
            }

            if (count === this.width) {
                for (let yy = fullY; yy < this.height; yy++) {
                    for (let x = 0; x < this.width; x++) {
                        this.isFilled[x][yy] = this.isFilled[x][yy+1];
                    }
                }
                for (let x = 0; x < this.width; x++) {
                    this.isFilled[x][this.height] = false;
                }
            }
        }
    }

    clearFullLines_Refactored(score) {
        for (let lineIndex = 1; lineIndex < this.height + 1; lineIndex++) {
            if (this.lineIsFull(lineIndex)) {
                score++;
                this.shiftLinesDown(lineIndex);
                lineIndex--;
                this.addEmptyLineOnTop();
            }
        }
        return score;
    }

    addEmptyLineOnTop()
    {
        throw new Error('not implemented addEmptyLineOnTop');
    }

    shiftLinesDown(lineIndex)
    {
        throw new Error(`not implemented shiftLinesDown, arg: ${lineIndex}`);
    }

    lineIsFull(y)
    {
        throw new Error(`not implemented lineIsFull, arg: ${y}`);
    }
}

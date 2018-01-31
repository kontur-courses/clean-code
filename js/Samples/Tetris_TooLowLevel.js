class Field {
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
                if (this.isFilled[y][x]) {
                    count++;
                    if (count === this.width) {
                        fullY = y;
                    }
                }
            }

            if (count === this.width) {
                for (let yy = fullY; yy < this.height; yy++) {
                    for (let x = 0; x < this.width; x++) {
                        this.isFilled[yy][x] = this.isFilled[yy+1][x];
                    }
                }
                for (let x = 0; x < this.width; x++) {
                    this.isFilled[this.height][x] = false;
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
        throw new Error();
    }

    shiftLinesDown(lineIndex)
    {
        throw new Error(lineIndex.ToString());
    }

    lineIsFull(y)
    {
        throw new Error(y.ToString());
    }
}

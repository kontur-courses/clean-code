package ru.kontur.courses.samples.tetris;

public class TetrisLowLevel {
    private final int height;
    private final int width;
    private final boolean[][] isFilled;

    public TetrisLowLevel(int height, int width, boolean[][] isFilled) {
        this.height = height;
        this.width = width;
        this.isFilled = isFilled;
    }

    public void clearFullLines() {
        for (var y = 0; y < height; y++) {
            var count = 0;
            var fullY = 0;
            for (var x = 0; x < width; x++)
                if (isFilled[x][y]) {
                    count++;
                    if (count == width) fullY = y;
                }
            if (count == width) {
                for (var yy = fullY; yy < height; yy++)
                    for (var x = 0; x < width; x++)
                        isFilled[x][yy] = isFilled[x][yy + 1];
                for (var x = 0; x < width; x++)
                    isFilled[x][height] = false;
            }
        }
    }

    public void clearFullLinesRefactored(Integer score) {
        for (var lineIndex = 1; lineIndex < height + 1; lineIndex++) {
            if (lineIsFull(lineIndex)) {
                score++;
                shiftLinesDown(lineIndex);
                lineIndex--;
                addEmptyLineOnTop();
            }
        }
    }

    private void addEmptyLineOnTop() {
        throw new UnsupportedOperationException();
    }

    private void shiftLinesDown(int lineIndex) {
        throw new UnsupportedOperationException(Integer.toString(lineIndex));
    }

    private boolean lineIsFull(int y) {
        throw new UnsupportedOperationException(Integer.toString(y));
    }
}

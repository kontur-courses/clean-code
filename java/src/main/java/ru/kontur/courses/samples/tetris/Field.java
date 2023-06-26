package ru.kontur.courses.samples.tetris;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

//good abstraction level
public class Field {

    private final List<Set<Integer>> filledCellsLineByLine;
    private final int height, width;
    private final int score;

    public Field(int width, int height, List<Set<Integer>> filledCellsLineByLine, int score) {
        this.width = width;
        this.height = height;
        this.score = score;
        this.filledCellsLineByLine = filledCellsLineByLine;
    }

    public Field clearFullLines() {
        var notFullLines = getAllNotFullLines();
        var clearedLinesCount = height - notFullLines.size();
        var newLinesArray = createNewLinesArray(clearedLinesCount, notFullLines);
        return new Field(width, height, newLinesArray, score + clearedLinesCount);
    }

    private List<Set<Integer>> createNewLinesArray(int emptyLinesCount, List<Set<Integer>> nonEmptyLines) {
        var list = new ArrayList<Set<Integer>>();
        for (int i = 0; i < emptyLinesCount; i++) {
            list.add(new HashSet<>());
        }

        nonEmptyLines.addAll(list);
        return nonEmptyLines;
    }

    private List<Set<Integer>> getAllNotFullLines() {
        return filledCellsLineByLine.stream().filter(it -> it.size() != width).toList();
    }
}

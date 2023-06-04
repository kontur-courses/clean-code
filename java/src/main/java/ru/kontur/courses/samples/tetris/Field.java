package ru.kontur.courses.samples.tetris;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.List;
import java.util.stream.Collectors;

//good abstraction level
public class Field {

    private final HashSet<Integer>[] filledCellsLineByLine;
    private final int height, width;
    private final int score;

    public Field(int width, int height, HashSet<Integer>[] filledCellsLineByLine, int score) {
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

    private HashSet<Integer>[] createNewLinesArray(int emptyLinesCount, List<HashSet<Integer>> nonEmptyLines) {
        var list = new ArrayList<HashSet<Integer>>();
        for (int i = 0; i < emptyLinesCount; i++) {
            list.add(new HashSet<>());
        }

        nonEmptyLines.addAll(list);
        return (HashSet<Integer>[]) nonEmptyLines.toArray();
    }

    private List<HashSet<Integer>> getAllNotFullLines() {
        return Arrays.stream(filledCellsLineByLine).filter(it -> it.size() != width).collect(Collectors.toList());
    }
}

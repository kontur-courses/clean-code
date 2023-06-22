package ru.kontur.courses.chess.solved;

import org.junit.jupiter.api.Test;
import ru.kontur.courses.chess.BoardParser;
import ru.kontur.courses.chess.PieceColor;

import java.io.File;
import java.io.FileInputStream;
import java.util.Arrays;
import java.util.Objects;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class ChessProblemSolvedTest {

    @Test
    public void allTests() throws Exception {
        var resource = Thread.currentThread().getContextClassLoader().getResource("chessTests");
        assert resource != null;
        var dir = new File(resource.toURI());
        var inFiles = Arrays.stream(Objects.requireNonNull(dir.listFiles())).filter(it -> it.getName().contains(".in")).toList();
        for (var file : inFiles) {
            var in = new FileInputStream(file);
            var boardLines = new String(in.readAllBytes()).split("\n");
            in.close();
            var board = new BoardParser().parseBoard(boardLines);
            var whiteStatus = new ChessProblemSolved(board).getStatusFor(PieceColor.WHITE);
            var answerFile = new File(file.getAbsolutePath().replace(".in", ".ans"));
            var answerFileInput = new FileInputStream(answerFile);
            var answerText = new String(answerFileInput.readAllBytes()).trim();
            answerFileInput.close();

            assertEquals(answerText, whiteStatus.toString().toLowerCase());
        }
        System.out.println("Tests passed: " + inFiles.size());
    }
}

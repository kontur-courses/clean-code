package ru.kontur.courses.chess;

import org.junit.jupiter.api.Test;

import java.io.File;
import java.io.FileInputStream;
import java.util.Arrays;
import java.util.Objects;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class ChessProblemTest {

    @Test
    public void repeatedMethodCallDoNotChangeBehaviour() throws Exception {
        var boardLines = new String[]{
                "        ",
                "        ",
                "        ",
                "   q    ",
                "    K   ",
                " Q      ",
                "        ",
                "        ",
        };
        ChessProblem.loadFrom(boardLines);
        ChessProblem.calculateChessStatus();
        assertEquals(ChessStatus.CHECK, ChessProblem.chessStatus);

        // Now check that internal board modifications during the first call do not change answer
        ChessProblem.calculateChessStatus();
        assertEquals(ChessStatus.CHECK, ChessProblem.chessStatus);
    }

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
            ChessProblem.loadFrom(boardLines);
            var answerFile = new File(file.getAbsolutePath().replace(".in", ".ans"));
            var answerFileInput = new FileInputStream(answerFile);
            var answerText = new String(answerFileInput.readAllBytes()).trim();
            answerFileInput.close();

            ChessProblem.calculateChessStatus();
            assertEquals(answerText, ChessProblem.chessStatus.toString().toLowerCase());
        }
        System.out.println("Tests passed: " + inFiles.size());
    }
}

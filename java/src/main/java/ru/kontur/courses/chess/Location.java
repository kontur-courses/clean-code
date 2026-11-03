package ru.kontur.courses.chess;

public record Location(int x, int y) {
    @Override
    public String toString() {
        return "(" + x + ", " + y + ')';
    }
}

package ru.kontur.courses.samples.pathfinder;

import java.awt.*;

public interface Maze {
    boolean insideMaze(Point location);

    boolean isFree(Point location);
}

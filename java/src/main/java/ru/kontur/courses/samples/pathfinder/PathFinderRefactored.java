package ru.kontur.courses.samples.pathfinder;

import java.awt.*;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedList;

public class PathFinderRefactored {
    public static Point getNextStepToTarget(Point source, Point target, Maze maze) {
        var queue = new LinkedList<Point>();
        var used = new HashSet<Point>();
        queue.add(target);
        used.add(target);
        while (!queue.isEmpty()) {
            var p = queue.poll();

            for (var neighbour : getNeighbours(p, maze)) {
                if (used.contains(neighbour)) continue;
                if (neighbour == source)
                    return p;
                queue.add(neighbour);
                used.add(neighbour);
            }
        }
        return source;
    }

    public static Maze generateRandomWorld() {
        throw new UnsupportedOperationException("TODO");
    }

    private static Iterable<Point> getNeighbours(Point from, Maze maze) {
        return Arrays.stream(
                new Point[]{
                        new Point(1, 0),
                        new Point(-1, 0),
                        new Point(0, 1),
                        new Point(0, -1)
                })
                .map(it -> new Point(from.x + it.x, from.y + it.y))
                .filter(maze::insideMaze)
                .filter(maze::isFree)
                .toList();
    }
}

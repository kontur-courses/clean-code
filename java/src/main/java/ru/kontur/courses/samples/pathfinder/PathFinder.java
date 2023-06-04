package ru.kontur.courses.samples.pathfinder;

import java.awt.*;
import java.util.*;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class PathFinder {
    public static Maze maze;
    private static final Queue<Point> queue = new LinkedList<>();
    private static final Set<Point> used = new HashSet<>();

    public static void generateRandomMaze() {
    }

    public static Point getNExtStepToTarget(Point source, Point target) {
        queue.clear();
        used.clear();
        queue.add(target);
        used.add(target);
        while (!queue.isEmpty()) {
            var p = queue.poll();
            for (var neighbour : getNeighbours(p)) {
                if (used.contains(neighbour)) continue;
                if (neighbour == source)
                    return p;
                queue.add(neighbour);
                used.add(neighbour);
            }
        }
        return source;
    }

    private static List<Point> getNeighbours(Point point) {
        return Arrays.stream(new Point[]{
                new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1)
        }).map(it -> new Point(point.x + it.x, point.y + it.y)).filter(
                it -> maze.insideMaze(it)
        ).filter(it -> maze.isFree(it)).collect(Collectors.toList());
    }
}

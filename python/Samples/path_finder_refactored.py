from dataclasses import dataclass
from typing import List


@dataclass
class Point:
    x: int
    y: int

    def __add__(self, other):
        return Point(self.x + other.x, self.y + other.y)

    def __str__(self):
        return f"({self.x}, {self.y})"


@dataclass
class Maze:
    inside_maze: list
    is_free: list


class PathFinder:
    @staticmethod
    def generate_random_world():
        max_x, max_y = 4, 4
        maze_points = [Point(x, y) for x in range(max_x) for y in range(max_y)]
        return maze_points, maze_points

    @staticmethod
    def get_path_to_target(maze: Maze, start: Point, target: Point):
        prev = {}
        source = start
        while source:
            if not prev.get(str(source)):
                prev[str(source)] = PathFinder.get_next_step_to_target(maze, source, target)
            if prev[str(source)]:
                yield prev[str(source)]
            source = prev[str(source)]

    @staticmethod
    def get_next_step_to_target(maze: Maze, source: Point, target: Point):
        queue = [target, ]
        used = [target, ]
        while len(queue) > 0:
            point = queue.pop(0)
            for neighbour in PathFinder.get_neighbours(point, maze):
                if neighbour in used:
                    continue
                if neighbour == source:
                    return point
                queue.append(neighbour)
                used.append(neighbour)

        return None

    @staticmethod
    def get_neighbours(point: Point, maze: Maze) -> List[Point]:
        offsets = [Point(0, 1), Point(1, 0), Point(-1, 0), Point(0, -1)]
        points = [point + offset for offset in offsets]
        return [
            point
            for point in points
            if point in maze.inside_maze and point in maze.is_free
        ]

from dataclasses import dataclass
from typing import List


MAZE = {}
QUEUE = []
USED = []
PREV = {}


@dataclass
class Point:
    x: int
    y: int

    def __add__(self, other):
        return Point(self.x + other.x, self.y + other.y)

    def __str__(self):
        return f"({self.x}, {self.y})"


class PathFinder:
    @staticmethod
    def generate_random_maze() -> None:
        max_x, max_y = 4, 4
        maze_points = [Point(x, y) for x in range(max_x) for y in range(max_y)]
        MAZE["insideMaze"], MAZE["isFree"] = maze_points, maze_points

    def get_path_to_target(self, start: Point, target: Point):
        global PREV
        PREV = {}
        source = start
        while source:
            if not PREV.get(str(source)):
                PREV[str(source)] = self.get_next_step_to_target(source, target)
            if PREV[str(source)]:
                yield PREV[str(source)]
            source = PREV[str(source)]

    def get_next_step_to_target(self, source: Point, target: Point):
        global QUEUE, USED
        QUEUE = [target, ]
        USED = [target, ]
        while len(QUEUE) > 0:
            point = QUEUE.pop(0)
            for neighbour in self.get_neighbours(point):
                if neighbour in USED:
                    continue
                if neighbour == source:
                    return point
                QUEUE.append(neighbour)
                USED.append(neighbour)

        return None

    @staticmethod
    def get_neighbours(point: Point) -> List[Point]:
        offsets = [Point(0, 1), Point(1, 0), Point(-1, 0), Point(0, -1)]
        points = [point + offset for offset in offsets]
        return [
            point
            for point in points
            if point in MAZE["insideMaze"] and point in MAZE["isFree"]
        ]

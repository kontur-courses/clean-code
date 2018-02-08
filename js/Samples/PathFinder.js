let maze = {};
let queue = [];
let used = [];
let prev = [];

class PathFinder {
    generateRandomMaze() {
        maze = { insideMaze: (location) => { /*...*/ }, isFree: (location) => { /*...*/ } };
    }

    *getPathToTarget(start, target) {
        let source = start;

        while (source) {
            if (prev[source.toString()] === undefined) {
                prev[source.toString()] = this.getNextStepToTarget(source, target);
            }

            yield prev[source.toString()];
            source = prev[source.toString()];
        }
    }

    getNextStepToTarget(source, target) {
        queue.length = 0;
        used.length = 0;
        queue.push(target);
        used.push(target);
        while (queue.length) {
            const p = queue.shift();
            for (const neighbour of this.getNeighbours(p)) {
                if (contains(used, neighbour)) continue;
                if (isPointsEqual(neighbour, source)) {
                    return p;
                }
                queue.push(neighbour);
                used.push(neighbour);
            }
        }
        return null;
    }

    getNeighbours(from) {
        return [{x: 1, y: 0}, {x: -1, y: 0}, {x: 0, y: 1}, {x: 0, y: -1}]
            .map(shift => ({ x: shift.x + from.x, y: shift.y + from.y }))
            .filter(maze.insideMaze)
            .filter(maze.isFree);
    }
}

function contains(arr, source) {
    return arr.some(isEqualTo(source));
}

function isPointsEqual(point1, point2) {
    return point1.x === point2.x && point1.y === point2.y;
}

function isEqualTo(point1) {
    return (point2) => isPointsEqual(point1, point2);
}
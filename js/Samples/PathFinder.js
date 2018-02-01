class PathFinder {
    maze = {};
    queue = [];
    used = [];

    generateRandomMaze() {
        this.maze = { insideMaze: (location) => { /*...*/ }, isFree: (location) => { /*...*/ } };
    }

    getNextStepToTarget(source, target) {
        this.queue.length = 0;
        this.used.length = 0;
        this.queue.push(target);
        this.used.push(target);
        while (this.queue.length) {
            const p = this.queue.shift();
            for (const neighbour of this.getNeighbours(p)) {
                if (contains(this.used, source)) continue;
                if (isPointsEqual(neighbour, source)) {
                    return p;
                }
                this.queue.push(neighbour);
                this.used.push(neighbour);
            }
        }
        return source;
    }

    getNeighbours(from) {
        return [{x: 1, y: 0}, {x: -1, y: 0}, {x: 0, y: 1}, {x: 0, y: -1}]
            .map(shift => ({ x: shift.x + from.x, y: shift.y + from.y }))
            .filter(this.maze.insideMaze)
            .filter(this.maze.isFree);
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
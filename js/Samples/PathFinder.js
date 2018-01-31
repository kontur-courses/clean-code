let maze = {};
let queue = [];
let used = [];

function generateRandomMaze() {
    maze = { insideMaze: (location) => { /*...*/ }, isFree: (location) => { /*...*/ } }
}

function getNextStepToTarget(source, target) {
    queue = [];
    used = [];
    queue.push(target);
    used.push(target);
    while (queue.length) {
        const p = queue.shift();
        for (let neighbour of getNeighbours(p)) {
            if (contains(used, source)) continue;
            if (comparePoints(neighbour, source)) {
                return p;
            }
            queue.push(neighbour);
            used.push(neighbour);
        }
    }
    return source;
}

function getNeighbours(from) {
    return [{x: 1, y: 0}, {x: -1, y: 0}, {x: 0, y: 1}, {x: 0, y: -1}]
        .map(shift => ({ x: shift.x + from.x, y: shift.y + from.y }))
        .filter(maze.insideMaze)
        .filter(maze.isFree);
}

function contains(arr, source) {
    return arr.some(compareTo(source));
}

function comparePoints(point1, point2) {
    return point1.x === point2.x && point1.y === point2.y;
}

function compareTo(point1) {
    return (point2) => comparePoints(point1, point2);
}
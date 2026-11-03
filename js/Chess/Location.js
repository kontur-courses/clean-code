export default class Location {
    constructor(x, y) {
        this.x = x;
        this.y = y;
    }

    toString = () => `(${this.x}, ${this.y})`;

    equals(otherLocation) {
        if (!otherLocation) return false;
        return otherLocation.x === this.x && otherLocation.y === this.y;
    }
}
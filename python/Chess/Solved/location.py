class Location:
    def __init__(self, x: int, y: int):
        self.x = x
        self.y = y

    def __str__(self):
        return f"({self.x}, {self.y})"

    def __eq__(self, other):
        if isinstance(other, Location):
            return self.x == other.x and self.y == other.y
        return False

    def __mul__(self, other):
        if isinstance(other, Location):
            return Location(self.x * other.x, self.y * other.y)
        elif isinstance(other, int):
            return Location(self.x * other, self.y * other)
        return self

    def __add__(self, other):
        if isinstance(other, Location):
            return Location(self.x + other.x, self.y + other.y)
        else:
            return self

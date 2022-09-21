class TetrisTooLowLevel:
    def __init__(self, width, height, filled):
        self.width = width
        self.height = height
        self.filled = filled

    def clear_full_lines(self):
        for y in range(self.height):
            full = all(self.filled[y])
            if not full: continue

            for yy in range(y, self.height-1):
                for x in range(self.width):
                    self.filled[yy][x] = self.filled[yy + 1][x]

            for x in range(self.width):
                self.filled[self.height-1][x] = False

    def clear_full_lines_refactored(self, score):
        for line_index in range(1, self.height + 1):
            if self.line_is_full(line_index):
                score += 1
                self.shift_lines_down(line_index)
                line_index -= 1
                self.add_empty_line_on_top()

        return score

    def add_empty_line_on_top(self):
        raise NotImplementedError()

    def shift_lines_down(self, line_index):
        raise NotImplementedError(line_index)

    def line_is_full(self, y):
        raise NotImplementedError(y)

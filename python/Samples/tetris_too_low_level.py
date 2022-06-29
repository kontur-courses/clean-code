class TetrisTooLowLevel:
    def __init__(self, width, height, is_filled):
        self.width = width
        self.height = height
        self.is_filled = is_filled

    def clear_full_lines(self):
        for y in range(self.height):
            count = 0
            full_y = 0

            for x in range(self.width):
                if self.is_filled[x][y]:
                    count += 1
                    if count == self.width:
                        full_y = y

            if count == self.width:
                for yy in range(full_y, self.height):
                    for x in range(self.width):
                        self.is_filled[x][yy] = self.is_filled[x][yy + 1]

                for x in range(self.width):
                    self.is_filled[x][self.height] = False

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

class Field:
    def __init__(self, width, height, filled_cells_line_by_line, score=0):
        self.width = width
        self.height = height
        self.score = score
        self.filled_cells_line_by_line = filled_cells_line_by_line

    def clear_full_lines(self):
        not_full_lines = self.get_all_not_full_lines()
        cleared_lines_count = self.height - len(not_full_lines)
        new_lines_array = self.create_new_lines_array(
            cleared_lines_count, not_full_lines
        )
        print(new_lines_array)
        return Field(
            self.width, self.height, new_lines_array, self.score + cleared_lines_count
        )

    def create_new_lines_array(self, empty_lines_count, non_empty_lines):
        empty_lines = [[False, ] * self.width] * empty_lines_count
        return non_empty_lines + empty_lines

    def get_all_not_full_lines(self):
        return [
            line for line in self.filled_cells_line_by_line if sum(line) != self.width
        ]

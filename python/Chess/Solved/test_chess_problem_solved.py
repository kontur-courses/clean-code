import glob
import unittest

from chess_problem_solved import ChessProblem
from board_parser import BoardParser
from piece_color import PieceColor


def read_board_lines_from_file(filename):
    with open(filename) as file:
        return file.read().strip().split("\n")


def read_answer_from_file(filename):
    with open(filename) as file:
        return file.read().strip()


class TestChessProblem(unittest.TestCase):
    def test_all(self):
        tests_count = 0
        for file in glob.glob("../ChessTests/*.in"):
            self.execute_test_on_file(file)
            tests_count += 1
        print(f"Tests passed: {tests_count}")

    def execute_test_on_file(self, filename):
        board = BoardParser().parse_board(read_board_lines_from_file(filename))
        expected_answer = read_answer_from_file(filename.replace(".in", ".ans"))
        white_status = ChessProblem(board).get_status_for(PieceColor.white)
        self.assertEqual(expected_answer, white_status)


if __name__ == "__main__":
    unittest.main()

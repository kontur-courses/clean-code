import glob
import unittest

from chess_problem import ChessProblem
from chess_status import ChessStatus


def read_board_lines_from_file(filename):
    with open(filename) as file:
        return file.read().strip().split("\n")


def read_answer_from_file(filename):
    with open(filename) as file:
        return file.read().strip()


class TestChessProblem(unittest.TestCase):
    def test_repeated_method_call_do_not_change_behaviour(self):
        board_lines = [
            "        ",
            "        ",
            "        ",
            "   q    ",
            "    K   ",
            " Q      ",
            "        ",
            "        ",
        ]
        ChessProblem.load_from(board_lines)
        ChessProblem.calculate_chess_status()
        self.assertEqual(ChessStatus.check, ChessProblem.chess_status)

        # Now check that internal board modifications during the first call
        # do not change answer
        ChessProblem.calculate_chess_status()
        self.assertEqual(ChessStatus.check, ChessProblem.chess_status)

    def test_all(self):
        tests_count = 0
        for file in glob.glob("ChessTests/*.in"):
            self.execute_test_on_file(file)
            tests_count += 1
        print(f"Tests passed: {tests_count}")

    def execute_test_on_file(self, filename):
        board_lines = read_board_lines_from_file(filename)
        ChessProblem.load_from(board_lines)
        expected_answer = read_answer_from_file(f"{filename.split('.')[0]}.ans")
        ChessProblem.calculate_chess_status()
        self.assertEqual(expected_answer, ChessProblem.chess_status)


if __name__ == "__main__":
    unittest.main()

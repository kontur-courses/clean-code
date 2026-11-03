import re
import piece_type
from board import Board
from piece import Piece
from piece_color import PieceColor

from typing import List, Union, NoReturn


class BoardParser:
    def parse_board(self, lines: List[List[str]]) -> Union[Board, NoReturn]:
        if len(lines) != 8:
            raise Exception("Should be exactly 8 lines")

        if any([len(line) != 8 for line in lines]):
            raise Exception("All lines should have 8 chars length")

        cells = []
        for line in lines:
            if not line:
                raise Exception("incorrect input")
            parsed_line = []
            for piece in line:
                parsed_line.append(self.parse_piece(piece))
            cells.append(parsed_line)

        return Board(cells)

    def parse_piece(self, piece_sign: str):
        color = (
            PieceColor.WHITE if re.search(r"[A-Z]", piece_sign) else PieceColor.BLACK
        )
        piece_type = self.parse_piece_type(piece_sign.upper())
        return Piece(piece_type, color) if piece_type else None

    @staticmethod
    def parse_piece_type(sign: str):
        sign_mapping = {
            "R": piece_type.Rook,
            "K": piece_type.King,
            "N": piece_type.Knight,
            "B": piece_type.Bishop,
            "Q": piece_type.Queen,
            " ": None,
            ".": None,
        }
        try:
            return sign_mapping[sign]
        except KeyError:
            raise Exception(f"Unknown chess piece {sign}")

from __future__ import annotations
from piece_color import PieceColor
from typing import TYPE_CHECKING
if TYPE_CHECKING:
    from board import Board
    from location import Location
    from piece_type import PieceType


class Piece:
    def __init__(self, piece_type: PieceType, color: PieceColor):
        self.piece_type = piece_type
        self.color = color

    def get_moves(self, location: Location, board: Board):
        return self.piece_type.get_movies(location, board)

    def __str__(self):
        c = f" {self.piece_type}" if self.piece_type else " ."
        return c.lower() if self.color == PieceColor.black else c

    def is_equal(self, color: PieceColor, piece_type: PieceType):
        is_color_equal = self.color == color
        if piece_type:
            return is_color_equal and self.piece_type == piece_type
        else:
            return is_color_equal

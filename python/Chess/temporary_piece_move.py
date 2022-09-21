from __future__ import annotations
from typing import TYPE_CHECKING
if TYPE_CHECKING:
    from board import Board
    from location import Location
    from piece import Piece


class TemporaryPieceMove:
    def __init__(
        self,
        board: Board,
        location_from: Location,
        location_to: Location,
        old_destination_piece: Piece,
    ):
        self.board = board
        self.location_from = location_from
        self.location_to = location_to
        self.old_destination_piece = old_destination_piece

    def undo(self):
        self.board.set(self.location_from, self.board.get_piece(self.location_to))
        self.board.set(self.location_to, self.old_destination_piece)

from __future__ import annotations
from typing import List, Optional
from location import Location
from temporary_piece_move import TemporaryPieceMove
from typing import TYPE_CHECKING
if TYPE_CHECKING:
    from piece_color import PieceColor
    from piece import Piece


class Board:
    def __init__(self, cells: List[List[Optional[Piece]]]):
        self.cells = cells

    def get_pieces(self, color: PieceColor):
        return [
            location
            for location in self.all_board()
            if self.get_piece(location) and self.get_piece(location).color == color
        ]

    def get_piece(self, location: Location):
        return self.cells[location.y][location.x] if self.contains(location) else None

    def set(self, location: Location, cell: Optional[Piece]):
        self.cells[location.y][location.x] = cell

    def perform_temporary_move(self, location_from: Location, location_to: Location):
        old = self.get_piece(location_to)
        self.set(location_to, self.get_piece(location_from))
        self.set(location_from, None)
        return TemporaryPieceMove(self, location_from, location_to, old)

    def all_board(self):
        result = []

        for y in range(len(self.cells)):
            for x in range(len(self.cells[0])):
                result.append(Location(x, y))

        return result

    def contains(self, location: Location):
        return 0 <= location.x < len(self.cells[0]) and 0 <= location.y < len(
            self.cells
        )

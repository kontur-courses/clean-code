from __future__ import annotations
from typing import List
from location import Location
from typing import TYPE_CHECKING
if TYPE_CHECKING:
    from board import Board


class PieceType:
    def __init__(self, infinite: bool, sign: str, directions: List[Location]):
        self.infinite = infinite
        self.sign = sign
        self.directions = get_permutations(directions)

    def get_movies(self, location_from: Location, board: Board):
        result = []
        for direction in self.directions:
            result.extend(self.moves_in_one_direction(location_from, board, direction))
        return result

    def moves_in_one_direction(
        self, location_from: Location, board: Board, direction: Location
    ):
        piece = board.get_piece(location_from)
        result = []
        distance = 1

        while distance:
            to = location_from + direction * distance
            if not board.contains(to):
                break
            destination_piece = board.get_piece(to)
            if destination_piece is None:
                result.append(to)
            else:
                if destination_piece.color != piece.color:
                    result.append(to)
                break
            distance = distance + 1 if self.infinite else 0
        return result

    def __str__(self):
        return self.sign


def get_permutations(directions: List[Location]) -> List[Location]:
    deltas = [
        Location(1, 1),
        Location(1, -1),
        Location(-1, 1),
        Location(-1, -1),
    ]

    permutations = []
    for direction in directions:
        for delta in deltas:
            current = direction * delta
            if current not in permutations:
                permutations.append(current)

    return permutations


Rook = PieceType(True, 'R', [Location(1, 0), Location(0, 1)])
King = PieceType(False, 'K', [Location(1, 1),  Location(1, 0),  Location(0, 1)])
Queen = PieceType(True, 'Q', [Location(1, 1),  Location(1, 0),  Location(0, 1)])
Bishop = PieceType(True, 'B', [Location(1, 1)])
Knight = PieceType(False, 'N', [Location(2, 1),  Location(1, 2)])

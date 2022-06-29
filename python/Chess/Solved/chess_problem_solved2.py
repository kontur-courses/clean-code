from itertools import chain

from piece_color import PieceColor
from chess_status import ChessStatus
from piece_type import King


class ChessProblem:
    def __init__(self, board):
        self.board = board

    def get_status_for(self, color):
        is_check = self.is_check_for(color)
        has_moves = self.has_safe_moves_for(color)
        if is_check:
            return ChessStatus.check if has_moves else ChessStatus.mate
        else:
            return ChessStatus.ok if has_moves else ChessStatus.stalemate

    def is_attack_king(self, move, color):
        piece = self.board.get_piece(move.loc_to)
        return bool(piece and piece.is_equal(color, King))

    def is_check_for(self, color):
        all_moves = self.get_all_moves_of(invert_color(color))
        return any([self.is_attack_king(move, color) for move in all_moves])

    def has_safe_moves_for(self, color):
        return any(map(self.is_safe_move, self.get_all_moves_of(color)))

    def get_all_moves_of(self, color):
        all_moves = [self.get_moves(loc) for loc in self.board.get_pieces(color)]
        return list(chain(*all_moves))

    def get_moves(self, piece_loc):
        moves = self.board.get_piece(piece_loc).get_moves(piece_loc, self.board)
        return [ChessMove(piece_loc, destination) for destination in moves]

    def is_safe_move(self, move):
        piece_color = self.board.get_piece(move.loc_from).color
        move = self.board.perform_temporary_move(move.loc_from, move.loc_to)
        is_valid = not self.is_check_for(piece_color)
        move.undo()
        return is_valid


class ChessMove:
    def __init__(self, loc_from, loc_to):
        self.loc_from = loc_from
        self.loc_to = loc_to


def invert_color(color):
    return PieceColor.black if color == PieceColor.white else PieceColor.white

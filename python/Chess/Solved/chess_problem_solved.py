from piece_color import PieceColor
from chess_status import ChessStatus
from piece_type import King


class ChessProblem:
    def __init__(self, board):
        self.board = board

    def get_status_for(self, color):
        is_check = self.is_check_for(color)
        has_moves = self.has_moves_for(color, self.is_valid_move)
        if is_check:
            return ChessStatus.check if has_moves else ChessStatus.mate
        else:
            return ChessStatus.ok if has_moves else ChessStatus.stalemate

    def is_check_for(self, color):
        is_attack_king = (
            lambda loc_from, loc_to: True
            if self.board.get_piece(loc_to)
            and self.board.get_piece(loc_to).is_equal(color, King)
            else False
        )
        return self.has_moves_for(invert_color(color), is_attack_king)

    def has_moves_for(self, color, is_acceptable_move):
        has_moves = False
        moves = self.board.get_pieces(color)
        for piece_loc in moves:
            for dest_loc in self.get_moves(piece_loc):
                if is_acceptable_move(piece_loc, dest_loc):
                    has_moves = True
                    break
            if has_moves:
                break
        return has_moves

    def get_moves(self, piece_loc):
        return self.board.get_piece(piece_loc).get_moves(piece_loc, self.board)

    def is_valid_move(self, loc_from, loc_to):
        piece_color = self.board.get_piece(loc_from).color
        move = self.board.perform_temporary_move(loc_from, loc_to)
        is_valid = not self.is_check_for(piece_color)
        move.undo()
        return is_valid


def invert_color(color):
    return PieceColor.black if color == PieceColor.white else PieceColor.white

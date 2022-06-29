from board_parser import BoardParser
from piece_color import PieceColor
from chess_status import ChessStatus
import piece_type


class ChessProblem:
    board = None
    chess_status = None

    @staticmethod
    def load_from(lines):
        ChessProblem.board = BoardParser().parse_board(lines)

    # Определяет мат, шах или пат белым.
    @staticmethod
    def calculate_chess_status():
        is_check = ChessProblem.is_check_for_white()
        has_movies = False

        for loc_from in ChessProblem.board.get_pieces(PieceColor.white):
            for loc_to in ChessProblem.board.get_piece(loc_from).get_moves(
                loc_from, ChessProblem.board
            ):
                old = ChessProblem.board.get_piece(loc_to)
                ChessProblem.board.set(loc_to, ChessProblem.board.get_piece(loc_from))
                ChessProblem.board.set(loc_from, None)
                if not ChessProblem.is_check_for_white():
                    has_movies = True
                ChessProblem.board.set(loc_from, ChessProblem.board.get_piece(loc_to))
                ChessProblem.board.set(loc_to, old)

        if is_check:
            if has_movies:
                ChessProblem.chess_status = ChessStatus.check  # шах
            else:
                ChessProblem.chess_status = ChessStatus.mate  # мат
        elif has_movies:
            ChessProblem.chess_status = ChessStatus.ok
        else:
            ChessProblem.chess_status = ChessStatus.stalemate  # пат

    # check — это шах
    @staticmethod
    def is_check_for_white():
        is_check = False
        for loc in ChessProblem.board.get_pieces(PieceColor.black):
            piece = ChessProblem.board.get_piece(loc)
            moves = piece.get_moves(loc, ChessProblem.board)
            for destination in moves:
                destination_piece = ChessProblem.board.get_piece(destination)
                if destination_piece and destination_piece.is_equal(
                    PieceColor.white, piece_type.King
                ):
                    is_check = True
        if is_check:
            return True
        return False

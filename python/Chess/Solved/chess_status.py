from enum import Enum


class ChessStatus(Enum):
    OK = "ok"
    """Никто не проиграл и не выиграл"""

    CHECK = "check"
    """Шах"""

    STALEMATE = "stalemate"
    """Пат"""

    MATE = "mate"
    """Мат"""

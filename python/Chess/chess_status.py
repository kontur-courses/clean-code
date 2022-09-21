import glob
from dataclasses import dataclass


@dataclass
class ChessStatus:
    ok = "ok"
    check = "check"  # шах
    stalemate = "stalemate"  # пат
    mate = "mate"  # мат

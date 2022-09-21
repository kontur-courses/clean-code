# Общий код, который можно реиспользовать
from itertools import cycle
from typing import Iterable, Iterator, List


def get_digits_from_least_significant(number: int) -> List[int]:
    digits = []

    while number > 0:
        digit = number % 10
        digits.append(digit)
        number = number // 10

    return digits


def sum_with_weights(digits: Iterable, weights: Iterable) -> int:
    return sum([digit * weight for digit, weight in zip(digits, weights)])


def repeat(items: List) -> Iterator:
    return cycle(items)

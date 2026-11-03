from control_digit import get_digits_from_least_significant, repeat, sum_with_weights


def upc(number: int) -> int:
    weights = repeat([3, 1])
    digits = get_digits_from_least_significant(number)
    m = sum_with_weights(digits, weights) % 10
    return 0 if m == 0 else 10 - m

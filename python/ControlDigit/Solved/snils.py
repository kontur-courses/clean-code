from control_digit import get_digits_from_least_significant, sum_with_weights


def snils(number: int) -> int:
    weights = list(range(1, 10))
    digits = get_digits_from_least_significant(number)
    result = sum_with_weights(digits, weights) % 101
    return 0 if result == 100 else result

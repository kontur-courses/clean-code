def isbn13(number: int) -> int:
    sum = 0
    factor = 1

    while number > 0:
        digit = number % 10
        sum += factor * digit
        factor = 4 - factor
        number = number // 10

    m = sum % 10
    if m == 0:
        return 0
    return 10 - m

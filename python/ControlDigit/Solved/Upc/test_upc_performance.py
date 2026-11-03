import time

from upc import upc


def upc_fast(number: int) -> int:
    sum = 0
    factor = 3
    while number > 0:
        digit = number % 10
        sum += factor * digit
        factor = 4 - factor
        number = number // 10
    m = sum % 10
    if m == 0:
        return 0
    return 10 - m


def test_upc_performance():
    count = 10000000

    start_time = time.time()
    for i in range(count):
        upc_fast(12345678)
    print(f"DoWhile : {time.time() - start_time}")

    start_time = time.time()
    for i in range(count):
        upc(12345678)
    print(f"CleanCode : {time.time() - start_time}")


test_upc_performance()

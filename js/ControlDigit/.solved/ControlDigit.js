// Общий код, который можно реиспользовать

export function getDigitsFromLeastSignificant (number) {
    const digits = [];
    do {
        const digit = number % 10 >> 0; // Быстрая альтернатива Math.floor(number % 10)
        digits.push(digit);
        number = number / 10;
    } while (number >= 1);

    return digits;
}

export function sumWithWeights (digits, weights) {
    const factoredDigits = digits.map((item, index) => item * weights[index]);
    return factoredDigits.reduce((sum, item) => sum + item);
}

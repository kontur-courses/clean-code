// Общий код, который можно реиспользовать

export function getDigitsFromLeastSignificant (number) {
    const digits = [];
    do {
        digits.push(number % 10);
        number = Math.floor(number / 10);
    } while (number > 0);

    return digits;
}

export function sumWithWeights (digits, weights) {
    const factoredDigits = digits.map((item, index) => item * weights[index]);
    return factoredDigits.reduce((sum, item) => sum + item);
}

const weights = Array.from(new Array(9), (_, index) => index + 1);

export default function controlNumber (number) {
    const digits = getDigitsFromLeastSignificant(number);
    const result = sumWithWeights(digits, weights) % 101;

    return result === 100 ? 0 : result;
}

function getDigitsFromLeastSignificant (number) {
    const digits = [];
    do {
        digits.push(number % 10);
        number = Math.floor(number / 10);
    } while (number > 0);

    return digits;
}

function sumWithWeights (digits, weights) {
    const factoredDigits = digits.map((item, index) => item * weights[index]);
    return factoredDigits.reduce((sum, item) => sum + item);
}
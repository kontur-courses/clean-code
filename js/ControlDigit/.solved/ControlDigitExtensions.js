export function controlDigit (number) {
    let sum = 0;
    let factor = 1;

    do {
        const digit = number % 10;
        sum += factor * digit;
        factor = 4 - factor;
        number = Math.floor(number / 10);
    } while (number > 0);


    let result = sum % 11;
    if (result === 10)
        result = 1;

    return result;
}

export function controlDigit2 (number) {
    const digits = getDigitsFromLeastSignificant(number);
    const weights = createWeights();
    const sum = sumWithWeights(digits, weights) % 11;
    return sum === 10 ? 1 : sum;
}


function getDigitsFromLeastSignificant (number) {
    const digits = [];
    do {
        digits.push(number % 10);
        number = Math.floor(number / 10);
    } while (number > 0);

    return digits;
}

function createWeights () {
    const generator = repeat([1, 3])
    return Array.from(new Array(20), () => generator.next().value);
}

function sumWithWeights (digits, weights) {
    const factoredDigits = digits.map((item, index) => item * weights[index]);
    return factoredDigits.reduce((sum, item) => sum + item);
}

function* repeat (items) {
    while (true) {
        for (let i = 0; i < items.length; i++) {
            yield items[i];
        }
    }
}
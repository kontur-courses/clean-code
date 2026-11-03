import { getDigitsFromLeastSignificant, sumWithWeights } from './ControlDigit'

const weights = createWeights();

export default function upc (number) {
    const digits = getDigitsFromLeastSignificant(number);
    const m = sumWithWeights(digits, weights) % 10;
    return m === 0 ? 0 : 10 - m;
}

function createWeights () {
    const generator = repeat([3, 1]);
    return Array.from(new Array(11), () => generator.next().value);
}

function* repeat (items) {
    while (true) {
        for (let i = 0; i < items.length; i++) {
            yield items[i];
        }
    }
}
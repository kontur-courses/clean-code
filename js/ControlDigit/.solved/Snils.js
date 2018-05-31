import { getDigitsFromLeastSignificant, sumWithWeights } from './ControlDigit'

const weights = Array.from(new Array(9), (_, index) => index + 1);

export default function controlNumber (number) {
    const digits = getDigitsFromLeastSignificant(number);
    const result = sumWithWeights(digits, weights) % 101;

    return result === 100 ? 0 : result;
}
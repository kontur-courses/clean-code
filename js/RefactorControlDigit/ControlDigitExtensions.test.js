import {controlDigit, controlDigit2} from './ControlDigitExtensions'

describe('ControlDigit', () => {
    [
        {input: 0, expectedResult: 0},
        {input: 1, expectedResult: 1},
        {input: 2, expectedResult: 2},
        {input: 9, expectedResult: 9},
        {input: 10, expectedResult: 3},
        {input: 15, expectedResult: 8},
        {input: 17, expectedResult: 1},
        {input: 18, expectedResult: 0},
        {input: 12345678, expectedResult: 2},
    ].forEach(({input, expectedResult}) => {
        test("should calculate control digit for " + input, () => {
            expect(controlDigit(input)).toBe(expectedResult);
        });
    });

    test('Compare implementations', () => {
        for (let i = 0; i < 10000; i++) {
            expect(controlDigit(i)).toBe(controlDigit2(i));
        }
    });
});

describe('ControlDigit performance tests', () => {
    test('Text ControlDigit speed', () => {
        const count = 1000000;
        console.time('Old');
        for (let i = 0; i < count; i++) {
            controlDigit(12345678);
        }
        console.timeEnd('Old');
        console.time('New');
        for (let i = 0; i < count; i++) {
            controlDigit2(12345678);
        }
        console.timeEnd('New');
    });
});

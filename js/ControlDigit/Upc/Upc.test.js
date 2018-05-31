import upc from './Upc'

describe('Upc', () => {
    [
        {input: 0, expectedResult: 0},
        {input: 1, expectedResult: 7},
        {input: 2, expectedResult: 4},
        {input: 9, expectedResult: 3},
        {input: 10, expectedResult: 9},
        {input: 13, expectedResult: 0},
        {input: 15, expectedResult: 4},
        {input: 17, expectedResult: 8},
        {input: 18, expectedResult: 5},
        {input: 11111111111, expectedResult: 7},
        {input: 12345678901, expectedResult: 2},
        {input: 98765432101, expectedResult: 2},
        {input: 11223344556, expectedResult: 2},
        {input: 32512312431, expectedResult: 1},
        {input: 98439874398, expectedResult: 8},
        {input: 98439876398, expectedResult: 6},
    ].forEach(({input, expectedResult}) => {
        test("should calculate control digit for " + input, () => {
            expect(upc(input)).toBe(expectedResult);
        });
    });
});

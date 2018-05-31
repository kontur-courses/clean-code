import controlNumber from './Snils'

describe('controlNumber', () => {
    test('should return number', () => {
        expect(typeof controlNumber(123)).toBe('number');
    });

    const testCases = [
        [1, 1],
        [10, 2],
        [100, 3],
        [1001, 4 + 1],
        [1111, 4 + 3 + 2 + 1],
        [112233445, 95],
        [87654303, 0],
        [87654302, 0],
        [116973385, 89],
        [152675138, 70],
        [463436384, 96],
        [158757369, 28],
        [192168000, 62],
    ];
    
    for (const testData of testCases) {
        test('should return right numbers', () => {
            expect(controlNumber(testData[0])).toBe(testData[1]);
        });
    }
});
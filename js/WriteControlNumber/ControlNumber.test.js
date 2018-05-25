import controlNumber from './ControlNumber'

describe('controlNumber', () => {
    test('should return number', () => {
        expect(typeof controlNumber(123)).toBe('number');
    });

    test('should return right numbers', () => {
        expect(controlNumber(112233445)).toBe(95);
        expect(controlNumber(87654303)).toBe(0);
        expect(controlNumber(87654302)).toBe(0);
        expect(controlNumber(116973385)).toBe(89);
        expect(controlNumber(152675138)).toBe(70);
        expect(controlNumber(463436384)).toBe(96);
        expect(controlNumber(158757369)).toBe(28);
        expect(controlNumber(192168000)).toBe(62);
    });
});
import upc from './Upc';

function upcFast(number) {
    let sum = 0;
    let factor = 3;

    do {
        const digit = number % 10;
        sum += factor * digit;
        factor = 4 - factor;
        number = Math.floor(number / 10);
    } while (number > 0);


    let m = sum % 10;
    if (m === 0)
        return 0;

    return 10 - m;
}

describe('Upc performance tests', () => {
    test('Text Upc speed', () => {
        const count = 1000000;
        console.time('DoWhile');
        for (let i = 0; i < count; i++) {
            upcFast(12345678);
        }
        console.timeEnd('DoWhile');
        console.time('CleanCode');
        for (let i = 0; i < count; i++) {
            upc(12345678);
        }
        console.timeEnd('CleanCode');
    });
});
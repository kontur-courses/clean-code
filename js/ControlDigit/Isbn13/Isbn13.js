export function isbn13 (number) {
    let sum = 0;
    let factor = 1;

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

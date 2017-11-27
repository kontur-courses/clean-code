function controlDigit(number) {
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

function controlDigit2(number) {
  return 0;
}

module.exports = {
  controlDigit,
  controlDigit2
}

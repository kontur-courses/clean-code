import {controlDigit, controlDigit2} from './ControlDigitExtensions'

test('ControlDigit', () => {
  expect(controlDigit(0)).toBe(0);
  expect(controlDigit(1)).toBe(1);
  expect(controlDigit(2)).toBe(2);
  expect(controlDigit(9)).toBe(9);
  expect(controlDigit(10)).toBe(3);
  expect(controlDigit(15)).toBe(8);
  expect(controlDigit(17)).toBe(1);
  expect(controlDigit(18)).toBe(0);
  expect(controlDigit(12345678)).toBe(2);
});


xtest('ControlDigit2', () => {
  expect(controlDigit2(0)).toBe(0);
  expect(controlDigit2(1)).toBe(1);
  expect(controlDigit2(2)).toBe(2);
  expect(controlDigit2(9)).toBe(9);
  expect(controlDigit2(10)).toBe(3);
  expect(controlDigit2(15)).toBe(8);
  expect(controlDigit2(17)).toBe(1);
  expect(controlDigit2(18)).toBe(0);
  expect(controlDigit2(12345678)).toBe(2);
});

test('Compare speed of implementation', () => {
  const count = 1000000;
  console.time('ControlDigit')
  for (let i = 0; i < count; i++) {
    controlDigit(12345678);
  }
  console.timeEnd('ControlDigit')
  console.time('ControlDigit2')
  for (let i = 0; i < count; i++) {
    controlDigit2(12345678);
  }
  console.timeEnd('ControlDigit2')
})

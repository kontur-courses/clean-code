package ru.kontur.courses.control.digit.isbn13;

public class Isb13 {
    public static int CalculateIsbn13(long number) {
        int sum = 0;
        int factor = 1;
        do {
            int digit = (int) (number % 10);
            sum += factor * digit;
            factor = 4 - factor;
            number /= 10;
        } while (number > 0);
        int m = sum % 10;
        if (m == 0) return 0;

        return 10 - m;
    }
}

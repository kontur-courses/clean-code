package ru.kontur.courses.control.digit.snils;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.assertEquals;


public class SnilsTest {
    @ParameterizedTest
    @CsvSource(value = {"1,1", "10,2", "100,3", "10001,5", "1111,10", "112233445,95", "87654303,0", "87654302,0", "116973385,89",
            "152675138,70", "463436384,96", "158757369,28", "192168000,62"})
    public void shouldCalculateSnilsSuccess(long input, long expected) {
        assertEquals(expected, Snils.calculate(input));
    }
}

package ru.kontur.courses.control.digit.upc;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class UpcTest {
    
    @ParameterizedTest
    @CsvSource(value = {
            "00000000000,0",
            "00000000001,7",
            "00000000002,4",
            "00000000009,3",
            "00000000010,9",
            "00000000013,0",
            "00000000015,4",
            "00000000017,8",
            "00000000018,5",
            "11111111111,7",
            "12345678901,2",
            "98765432101,2",
            "11223344556,2",
            "32512312431,1",
            "98439874398,8",
            "98439876398,6",
    })
    public void upc(long input, int expected) {
        final var actual = Upc.calculateUpc(input);
        assertEquals(expected, actual);
    }
}

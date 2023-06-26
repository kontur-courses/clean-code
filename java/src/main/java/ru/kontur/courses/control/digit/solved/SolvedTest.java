package ru.kontur.courses.control.digit.solved;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class SolvedTest {

    @ParameterizedTest
    @CsvSource(value = {
            "0,0",
            "1,7",
            "2,4",
            "9,3",
            "10,9",
            "13,0",
            "15,4",
            "17,8",
            "18,5",
            "11111111111,7",
            "12345678901,2",
            "98765432101,2",
            "11223344556,2",
            "32512312431,1",
            "98439874398,8",
            "98439876398,6"
    })
    public void shouldCalculateUpcSuccess(long input, long expected) {
        assertEquals(expected, Solved.calculateUpc(input));
    }

    @ParameterizedTest
    @CsvSource(
            value = {
                    "1,1",
                    "10,2",
                    "100,3",
                    "1001,5",
                    "1111,10",
                    "112233445,95",
                    "87654303,0",
                    "87654302,0",
                    "116973385,89",
                    "152675138,70",
                    "463436384,96",
                    "158757369,28",
                    "192168000,62",
            }
    )
    public void shouldCalculateSnilsSuccess(long input, long expected) {
        assertEquals(expected, Solved.calculateSnils(input));
    }

    @Test
    public void shouldCompareImplementations() {
        for (int i = 0; i < 100000; i++) {
            assertEquals(Solved.calculateUpcOld(i), Solved.calculateUpc(i));
        }
    }

    @Test
    public void upcPerfomance() {
        var count = 10000000;
        var startTime = System.currentTimeMillis();
        for (int i = 0; i < count; i++) {
            Solved.calculateUpcOld(12345678);
        }
        System.out.println("Old " + (System.currentTimeMillis() - startTime));
        startTime = System.currentTimeMillis();
        for (int i = 0; i < count; i++) {
            Solved.calculateUpc(12345678);
        }
        System.out.println("New " + (System.currentTimeMillis() - startTime));
    }


}

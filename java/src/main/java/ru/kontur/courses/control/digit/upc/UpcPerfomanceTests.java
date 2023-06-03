package ru.kontur.courses.control.digit.upc;

import org.junit.jupiter.api.Test;

public class UpcPerfomanceTests {
    private static int calculateUpcFact(long number) {
        int sum = 0;
        int factor = 3;
        do
        {
            int digit = (int)(number % 10);
            sum += factor * digit;
            factor = 4 - factor;
            number /= 10;
        }
        while (number > 0);
        int m = sum % 10;
        if (m == 0)
            return 0;
        return 10 - m;
    }

    @Test
    public void upcPerfomance() {
        var count = 10000000;
        var sw = System.currentTimeMillis();

        for (int i = 0; i < count; i++)
            calculateUpcFact(12345678L);
        System.out.println("DoWhile "+(System.currentTimeMillis()-sw));
        sw = System.currentTimeMillis();
        for (int i = 0; i < count; i++)
            Upc.calculateUpc(12345678L);
        System.out.println("CleanCode "+ (System.currentTimeMillis() - sw));
    }
}

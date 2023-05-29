package ru.kontur.courses.control.digit.solved;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

public class Solved {
    public static int calculateSnils(long number) {
        var sum = sumWithWeights(getDigitsFromLeastSignificant(number), IntStream.rangeClosed(1, 9).boxed().collect(Collectors.toList())) % 101;

        return sum == 100 ? 0 : sum;
    }

    public static int calculateUpcOld(long number) {
        int sum = 0;
        int factor = 3;
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

    public static int calculateUpc(long number) {
        var m = sumWithWeights(getDigitsFromLeastSignificant(number), List.of(3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3)) % 10;
        return m == 0 ? 0 : 10 - m;
    }

    private static List<Integer> getDigitsFromLeastSignificant(long number) {
        var result = new ArrayList<Integer>();
        do {
            result.add((int) number % 10);
            number /= 10;
        } while (number > 0);

        return result;
    }

    private static int sumWithWeights(List<Integer> numbers, List<Integer> weights) {
        return zipJava9(numbers, weights).stream().map(it -> it.getKey() * it.getValue()).reduce(0, Integer::sum);
    }

    public static <A, B> List<Map.Entry<A, B>> zipJava9(List<A> as, List<B> bs) {
        return IntStream.range(0, Math.min(as.size(), bs.size()))
                .mapToObj(i -> Map.entry(as.get(i), bs.get(i)))
                .collect(Collectors.toList());
    }
}

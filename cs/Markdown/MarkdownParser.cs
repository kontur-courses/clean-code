using Markdown.Nodes;

namespace Markdown;

public static class MarkdownParser
{

    public static IReadOnlyList<BlockNode> Parse(string markdown)
    {
        //сплитуем markdown на строки
        //для каждой вызывает ParseBlock
        //Получаем распарсенный BlockNode
        //Добавляем его в список

        throw new NotImplementedException();
    }

    private static BlockNode ParseBlock(string line)
    {
        //Определяем тип блока (заголовок или параграф)
        //Передаем строку (с учетом сдвига "# ") в ParseInline
        //Получаем список InlineNode и создаем соответствующий BlockNode

        throw new NotImplementedException();
    }

    private static IReadOnlyList<InlineNode> ParseInline(string text, int startIndex = 0)
    {
        //разбиваем текст на токены c помощью InlineTokenizer
        //вызываем FindDelimiterTokenPairs для поиска пар токенов
        //вызываем BuildNodes для построения InlineNode по найденным парам
        throw new NotImplementedException();
    }

    internal static IReadOnlyList<DelimiterPair> FindDelimiterTokenPairs(
        IReadOnlyList<Token> tokens,
        string text)
    {
        var pairs = new List<DelimiterPair>();

        var stackOpenStrong = new Stack<DelimiterRunToken>();
        var stackOpenEmphasis = new Stack<DelimiterRunToken>();

        foreach (var token in tokens.OfType<DelimiterRunToken>())
        {
            var openStrong = stackOpenStrong.Count > 0
                ? stackOpenStrong.Peek()
                : null;

            var openEmphasis = stackOpenEmphasis.Count > 0
                ? stackOpenEmphasis.Peek()
                : null;

            var preferredEmphasis = openEmphasis is not null
                                    && (openStrong is null || openEmphasis.Index > openStrong.Index);

            foreach (var delimiter in SplitDelimiter(token, preferredEmphasis))
            {
                var stack = delimiter.Length == 2 ? stackOpenStrong : stackOpenEmphasis;

                if (delimiter.CanClose && stack.Count > 0)
                {
                    var opening = stack.Pop();
                    pairs.Add(new DelimiterPair(opening, delimiter));
                }
                else if (delimiter.CanOpen)
                {
                    stack.Push(delimiter);
                }
            }

        }

        return FilterValidPairs(pairs, text);
    }

    private static IReadOnlyList<DelimiterPair> FilterValidPairs(
        IReadOnlyList<DelimiterPair> pairs, string text)
    {
        return
        [
            .. pairs.Where(pair =>
                (pair.Opening.Index + pair.Opening.Length != pair.Closing.Index)
                && !IsInvalidWordPair(pair, text)
                && !pairs.Any(other => HasInvalidRelation(pair, other)))
        ];
    }

    private static bool HasInvalidRelation(
        DelimiterPair pair,
        DelimiterPair other)
    {
        var start = pair.Opening.Index;
        var end = pair.Closing.Index + pair.Closing.Length;

        var otherStart = other.Opening.Index;
        var otherEnd = other.Closing.Index + other.Closing.Length;

        var crosses =
            (start < otherStart && otherStart < end && end < otherEnd) ||
            (otherStart < start && start < otherEnd && otherEnd < end);

        var strongInsideEmphasis =
            pair.Opening.Length == 2 &&
            other.Opening.Length == 1 &&
            otherStart < start &&
            end < otherEnd;

        var isNestedWithinSameLength =
            pair.Opening.Length == other.Opening.Length &&
            other.Opening.Index < pair.Opening.Index &&
            other.Closing.Index > pair.Closing.Index;

        return crosses || strongInsideEmphasis || isNestedWithinSameLength;
    }

    private static bool IsInvalidWordPair(DelimiterPair pair, string text)
    {
        var openingInsideWord = IsInsideWord(pair.Opening, text);
        var closingInsideWord = IsInsideWord(pair.Closing, text);

        var betweenText = text.AsSpan(
            pair.Opening.Index + pair.Opening.Length,
            pair.Closing.Index - pair.Opening.Index - pair.Opening.Length);

        return (openingInsideWord || closingInsideWord)
               &&  betweenText.IndexOf(' ') >= 0;
    }

    private static bool IsInsideWord(Token token, string text)
    {
        var start = token.Index;
        var end = start + token.Length;

        return start > 0 &&
               end < text.Length &&
               char.IsLetter(text[start - 1]) &&
               char.IsLetter(text[end]);
    }

    private static List<DelimiterRunToken> SplitDelimiter(
        DelimiterRunToken delimiterRun,
        bool preferredEmphasisFirst)
    {
        var result = new List<DelimiterRunToken>();
        var index = delimiterRun.Index;
        var remaining = delimiterRun.Length;

        if (remaining % 2 != 0 && preferredEmphasisFirst)
        {
            result.Add(delimiterRun with { Index = index, Length = 1 });
            index++;
            remaining--;
        }

        while (remaining >= 2)
        {
            result.Add(delimiterRun with { Index = index, Length = 2 });
            index += 2;
            remaining -= 2;
        }

        if (remaining == 1)
        {
            result.Add(delimiterRun with { Index = index, Length = 1 });
        }

        return result;
    }

    private static IReadOnlyList<InlineNode> BuildNodes(
        string text,
        int startIndex,
        IReadOnlyList<Token> tokens,
        IReadOnlyList<DelimiterPair> pairs)
    {
        // Строим дерево по подтверждённым парам.
        // Обычный текст сохраняем в TextNode.
        // По Escape убираем экранирующие слеши из текста узлов.
        throw new NotImplementedException();
    }
}
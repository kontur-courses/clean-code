namespace Markdown.Parsing;

internal enum DelimiterType
{
    Emphasis,
    Strong
}

/// закрывающий маркер и тип выделения для пары
internal readonly record struct DelimiterPair(
    int ClosingIndex,
    DelimiterType Type);

/// находит пары корректных маркеров _ и __ в токенах
internal sealed class InlineDelimiterMatcher
{
    // Одинарное выделение может быть вложено только в двойное, поэтому открытых маркеров не больше двух
    private const int MaxOpenDelimiters = 2;

    private readonly IReadOnlyList<Token> tokens;
    private readonly DelimiterPair?[] pairs;
    private readonly List<OpenDelimiter> openDelimiters =
        new(MaxOpenDelimiters);

    private InlineDelimiterMatcher(IReadOnlyList<Token> tokens)
    {
        this.tokens = tokens;
        pairs = new DelimiterPair?[tokens.Count];
    }

    /// <summary>
    /// возвращает массив той же длины, что и tokens
    /// для открывающего маркера пары содержит его описание,
    /// для всех остальных токенов - <c>null</c>.
    /// </summary>
    public static DelimiterPair?[] FindPairs(IReadOnlyList<Token> tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        return new InlineDelimiterMatcher(tokens).Match();
    }

    private DelimiterPair?[] Match()
    {
        for (var i = 0; i < tokens.Count; i++)
        {
            if (GetDelimiterType(tokens[i]) is { } type)
                ProcessDelimiter(i, type);
        }

        return pairs;
    }

    private void ProcessDelimiter(int index, DelimiterType type)
    {
        var openPosition = FindOpenDelimiter(type);

        if (openPosition >= 0)
            TryClose(index, openPosition);
        else
            TryOpen(index, type);
    }

    private void TryOpen(int index, DelimiterType type)
    {
        if (!IsAllowedAtEmphasisEdge(GetNextCharacter(index)))
            return;

        // т.к. двойное выделение не может начаться внутри одинарного
        if (type == DelimiterType.Strong &&
            FindOpenDelimiter(DelimiterType.Emphasis) >= 0)
        {
            return;
        }

        openDelimiters.Add(new OpenDelimiter(index, type));
    }

    private void TryClose(int index, int openPosition)
    {
        if (!IsAllowedAtEmphasisEdge(GetPreviousCharacter(index)))
            return;

        if (openPosition != openDelimiters.Count - 1)
        {
            // если закрывается внешний маркер и внутри него остаётся
            // другой открытый маркер, то выделения пересекаются
            // Тогда все маркеры считаются обычным текстом
            openDelimiters.RemoveRange(
                openPosition,
                openDelimiters.Count - openPosition);

            return;
        }

        var opening = openDelimiters[openPosition];
        openDelimiters.RemoveAt(openPosition);

        if (CanFormPair(opening.TokenIndex, index))
            pairs[opening.TokenIndex] = new DelimiterPair(index, opening.Type);
    }

    /// <summary>
    /// выделение не должно выходить за границы одного слова если маркер внутри него
    /// </summary>
    private bool CanFormPair(int openingIndex, int closingIndex)
    {
        var isOpeningInsideWord =
            IsWordCharacter(GetPreviousCharacter(openingIndex));

        var isClosingInsideWord =
            IsWordCharacter(GetNextCharacter(closingIndex));

        if (!isOpeningInsideWord && !isClosingInsideWord)
            return true;

        return !HasWhitespaceBetween(openingIndex, closingIndex);
    }

    // вложенность не больше двух, поэтому каждый токен просматривается
    // не более двух раз и вся сложность остаётся линейной
    private bool HasWhitespaceBetween(int openingIndex, int closingIndex)
    {
        for (var i = openingIndex + 1; i < closingIndex; i++)
        {
            if (ContainsWhitespace(tokens[i].Value))
                return true;
        }

        return false;
    }

    private int FindOpenDelimiter(DelimiterType type)
    {
        for (var i = openDelimiters.Count - 1; i >= 0; i--)
        {
            if (openDelimiters[i].Type == type)
                return i;
        }

        return -1;
    }

    private char? GetPreviousCharacter(int tokenIndex)
    {
        if (tokenIndex == 0)
            return null;

        return tokens[tokenIndex - 1].Value[^1];
    }

    private char? GetNextCharacter(int tokenIndex)
    {
        if (tokenIndex + 1 >= tokens.Count)
            return null;

        return tokens[tokenIndex + 1].Value[0];
    }

    private static DelimiterType? GetDelimiterType(Token token)
    {
        if (token.Kind != TokenKind.UnderscoreRun)
            return null;

        return token.Value.Length switch
        {
            1 => DelimiterType.Emphasis,
            2 => DelimiterType.Strong,
            _ => null
        };
    }

    private static bool IsWordCharacter(char? character)
    {
        return character is { } value && !char.IsWhiteSpace(value);
    }

    // выделение не может начинаться перед пробелом и заканчиваться после него
    // а также не может примыкать к цифровому символу
    private static bool IsAllowedAtEmphasisEdge(char? character)
    {
        return character is { } value &&
               !char.IsWhiteSpace(value) &&
               !char.IsDigit(value);
    }

    private static bool ContainsWhitespace(string value)
    {
        foreach (var character in value)
        {
            if (char.IsWhiteSpace(character))
                return true;
        }

        return false;
    }

    private readonly record struct OpenDelimiter(
        int TokenIndex,
        DelimiterType Type);
}

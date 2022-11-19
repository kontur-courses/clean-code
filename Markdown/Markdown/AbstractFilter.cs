namespace Markdown;

public abstract class AbstractFilter
{
    public abstract List<Token> Filter(List<Token> tokens);

    protected static bool IsAtSide(IReadOnlyList<Token> tokens, int index, bool isAtLeftSide)
    {
        var isLeftSpace = index == 0 || tokens[index - 1].TokensType == TokenType.Space;
        var isRightSpace = index == tokens.Count - 1 || tokens[index + 1].TokensType == TokenType.Space;
        return isAtLeftSide ? isLeftSpace && !isRightSpace : !isLeftSpace && isRightSpace;
    }

    protected static bool HasOnlyTextBeforeEnding(IReadOnlyList<Token> tokens, int openingIndex, int endingIndex)
    {
        for (var i = openingIndex + 1; i < endingIndex; i++)
            if (tokens[i].TokensType != TokenType.Text)
                return false;
        return true;
    }

    protected static int GetEndingIndex(IReadOnlyList<Token> tokens, int openingIndex)
    {
        for (var i = openingIndex + 1; i < tokens.Count; i++)
            if (tokens[i].TokensType == tokens[openingIndex].TokensType)
                return i;
        return -1;
    }

    protected static List<int> GetPositions(IReadOnlyList<Token> tokens, TokenType type)
    {
        var positions = new List<int>();
        for (var i = 0; i < tokens.Count; i++)
            if (tokens[i].TokensType == type)
                positions.Add(i);
        return positions;
    }

    protected static void ToText(IReadOnlyList<Token> tokens, int start, int end)
    {
        for (var i = start; i <= end; i++)
            tokens[i].TokensType = TokenType.Text;
    }

    protected static void ToText(IReadOnlyList<Token> tokens, IEnumerable<int> positions)
    {
        foreach (var position in positions)
            tokens[position].TokensType = TokenType.Text;
    }

    protected static bool HasDigits(IReadOnlyList<Token> tokens, int start, int end)
    {
        for (var i = start; i <= end; i++)
            if (HasDigits(tokens[i]))
                return true;

        return false;
    }

    private static bool HasDigits(Token tokens)
    {
        return tokens.Text.Any(char.IsDigit);
    }
}
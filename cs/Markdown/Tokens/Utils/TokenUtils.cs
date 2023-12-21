using Markdown.Tokens.Types;

namespace Markdown.Tokens.Utils;

public static class TokenUtils
{
    public static readonly TokenTypeEqualityComparer TokenTypeEqualityComparer = new();
    
    public static bool IsTokenSurroundedWith(Token token, string str, Func<char, bool> condition, bool negateCondition)
    {
        var left = IsPrecededBySymbol(token, str, condition) ^ negateCondition;
        var right = IsFollowedBySymbol(token, str, condition) ^ negateCondition;

        return left && right;
    }

    public static bool IsInWord(Token token, string line)
    {
        if (IsTokenFirstOrLastInString(token, line.Length))
            return false;
        
        return !char.IsWhiteSpace(line[token.StartingIndex - 1]) &&
               !char.IsWhiteSpace(line[token.StartingIndex + token.Length]);
    }

    public static T? FindTokenPair<T>(int tokenIndex, List<T> tokens) where T : Token
    {
        if (tokenIndex < 0 || tokenIndex >= tokens.Count)
            throw new IndexOutOfRangeException("Provided tokenIndex was out of range.");
        if (!tokens[tokenIndex].Type.SupportsClosingTag)
            throw new ArgumentException("Provided token is not a pair token.");

        T? pair = null;
        if (tokens[tokenIndex].IsClosingTag && tokenIndex != 0)
        {
            for (var i = tokenIndex - 1; i >= 0; i--)
            {
                if (tokens[i].IsClosingTag ||
                    !TokenTypeEqualityComparer.Equals(tokens[i].Type, tokens[tokenIndex].Type))
                    continue;
                pair = tokens[i];
                break;
            }
        }

        if (tokens[tokenIndex].IsClosingTag || tokenIndex == tokens.Count - 1)
            return pair;

        for (var i = tokenIndex + 1; i < tokens.Count; i++)
        {
            if (!tokens[i].IsClosingTag || !TokenTypeEqualityComparer.Equals(tokens[i].Type, tokens[tokenIndex].Type))
                continue;
            pair = tokens[i];
            break;
        }

        return pair;
    }
    
    public static bool IsPrecededBySymbol(Token token, string line, Func<char, bool> condition)
        => token.StartingIndex > 0 && condition(line[token.StartingIndex - 1]);

    public static bool IsFollowedBySymbol(Token token, string line, Func<char, bool> condition)
        => token.StartingIndex + token.Length < line.Length && condition(line[token.StartingIndex + token.Length]);
    
    public static bool HasSymbolInBetween(string line, char symbol, int start, int end)
    {
        if (start < 0 || end >= line.Length || start > end)
            return false;
        for (var i = start; i <= end; i++)
            if (line[i] == symbol)
                return true;
        return false;
    }

    private static bool IsTokenFirstOrLastInString(Token token, int stringLength)
        => token.StartingIndex == 0 || token.StartingIndex + token.Length == stringLength;
}
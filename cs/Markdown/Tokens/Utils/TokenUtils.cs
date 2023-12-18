using Markdown.Tokens.Types;

namespace Markdown.Tokens.Utils;

public static class TokenUtils
{
    public static readonly TokenTypeEqualityComparer TokenTypeEqualityComparer = new();
    
    public static List<Token> DeleteMarkedTokens(IEnumerable<Token> tokens)
        => tokens
            .Where(t => !t.IsMarkedForDeletion)
            .ToList();
    
    public static bool IsTokenSurroundedWith(Token token, string str, Func<char, bool> condition, bool negateCondition)
    {
        var left = IsPrecededBySymbol(token, str, condition) ^ negateCondition;
        var right = IsFollowedBySymbol(token, str, condition) ^ negateCondition;

        return left && right;
    }
    
    public static Dictionary<string, List<Token>> CreatePairedTypesDictionary(IEnumerable<Token> tokens)
    {
        var types = new Dictionary<string, List<Token>>();

        foreach (var token in tokens.Where(currentToken => currentToken.Type.SupportsClosingTag))
        {
            if (!types.ContainsKey(token.Type.Value))
                types.Add(token.Type.Value, new List<Token>());
            types[token.Type.Value].Add(token);
        }

        return types;
    }

    public static bool IsInWord(Token token, string line)
    {
        if (IsTokenFirstOrLastInString(token, line.Length))
            return false;
        
        return !char.IsWhiteSpace(line[token.StartingIndex - 1]) &&
               !char.IsWhiteSpace(line[token.StartingIndex + token.Length]);
    }
    
    public static Token? FindTokenPair(int tokenIndex, List<Token> tokens)
    {
        if (tokenIndex < 0 || tokenIndex >= tokens.Count)
            throw new IndexOutOfRangeException("Provided tokenIndex was out of range.");
        if (!tokens[tokenIndex].Type.SupportsClosingTag)
            throw new ArgumentException("Provided token is not a pair token.");

        Token? pair = null;
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

    public static void FindPairAndMarkBothForDeletion(int tokenIndex, List<Token> tokens)
    {
        if (tokenIndex < 0 || tokenIndex >= tokens.Count)
            throw new IndexOutOfRangeException("Provided tokenIndex was out of range.");

        tokens[tokenIndex].IsMarkedForDeletion = true;
        var pair = FindTokenPair(tokenIndex, tokens);
        if (pair is not null)
            pair.IsMarkedForDeletion = true;
    }

    public static bool IsFollowedBySymbol(Token token, string line, Func<char, bool> condition)
        => token.StartingIndex + token.Length < line.Length && condition(line[token.StartingIndex + token.Length]);
    
    public static List<Token> GetPairedTokens(IEnumerable<Token> tokens)
        => tokens
            .Where(currentToken => currentToken.Type.SupportsClosingTag)
            .ToList();
    
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
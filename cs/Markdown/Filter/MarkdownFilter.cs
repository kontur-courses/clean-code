using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace Markdown.Filter;

public class MarkdownFilter : ITokenFilter
{
    private static readonly TokenTypeEqualityComparer TokenTypeEqualityComparer = new();

    public List<Token> FilterTokens(List<Token> tokens, string line)
    {
        var withoutEmptyLines = FilterEmptyLines(tokens);
        var withoutBreakingNumbers = FilterBreakingNumbers(withoutEmptyLines, line);
        var withoutDifferentWords = FilterDifferentWords(withoutBreakingNumbers, line);
        var withoutIncorrectlyNested = FilterNested(withoutDifferentWords, new StrongToken(), new EmphasisToken());
        var withoutSpaceSymbolInterruption = FilterSpaceInterruption(withoutIncorrectlyNested, line);
        var withoutDifferentPairTagsIntersection = FilterPairTagsIntersection(withoutSpaceSymbolInterruption);
        var finalResult = FilterUnpairedTags(withoutDifferentPairTagsIntersection);

        return finalResult;
    }

    private static List<Token> GetPairTags(List<Token> tokens)
        => tokens
            .Where(currentToken => currentToken.Type.SupportsClosingTag)
            .ToList();

    private static List<Token> FilterUnpairedTags(List<Token> tokens)
    {
        var pairTags = GetPairTags(tokens);

        for (var i = 0; i < pairTags.Count; i++)
        {
            var pair = FindTokenPair(i, pairTags);
            if (pair is null)
                pairTags[i].IsMarkedForDeletion = true;
        }

        return DeleteMarkedTokens(tokens);
    }
    
    private static List<Token> FilterPairTagsIntersection(List<Token> tokens)
    {
        var pairTags = GetPairTags(tokens);

        for (var i = 0; i < pairTags.Count - 3; i++)
        {
            if (!DifferentPairsIntersect(pairTags[i], pairTags[i + 1], pairTags[i + 2], pairTags[i + 3]))
                continue;

            pairTags[i].IsMarkedForDeletion = true;
            pairTags[i + 1].IsMarkedForDeletion = true;
            pairTags[i + 2].IsMarkedForDeletion = true;
            pairTags[i + 3].IsMarkedForDeletion = true;
        }

        return DeleteMarkedTokens(tokens);
    }

    private static bool DifferentPairsIntersect(Token token1, Token token2, Token token3, Token token4)
        => TokenTypeEqualityComparer.Equals(token1.Type, token3.Type)
           && TokenTypeEqualityComparer.Equals(token2.Type, token4.Type)
           && !TokenTypeEqualityComparer.Equals(token1.Type, token2.Type)
           && !token1.IsClosingTag
           && !token2.IsClosingTag
           && token3.IsClosingTag
           && token4.IsClosingTag;


    private static void FindPairAndMarkBothForDeletion(int tokenIndex, List<Token> tokens)
    {
        if (tokenIndex < 0 || tokenIndex >= tokens.Count)
            throw new IndexOutOfRangeException("Provided tokenIndex was out of range.");

        tokens[tokenIndex].IsMarkedForDeletion = true;
        var pair = FindTokenPair(tokenIndex, tokens);
        if (pair is not null)
            pair.IsMarkedForDeletion = true;
    }

    private static bool IsFollowedBySymbol(Token token, string line, Func<char, bool> condition)
        => token.StartingIndex + token.Length < line.Length && condition(line[token.StartingIndex + token.Length]);

    //если в паре за откр. тегом следует пробел или перед закр. тегом идет пробел, то удаляем всю пару
    private static List<Token> FilterSpaceInterruption(List<Token> tokens, string line)
    {
        var types = CreatePairedTypesDictionary(tokens);

        foreach (var type in types)
        {
            for (var i = 0; i < type.Value.Count; i++)
            {
                if (!type.Value[i].IsClosingTag && IsFollowedBySymbol(type.Value[i], line, char.IsWhiteSpace))
                    FindPairAndMarkBothForDeletion(i, type.Value);

                if (type.Value[i].IsClosingTag && IsPrecededBySymbol(type.Value[i], line, char.IsWhiteSpace))
                    FindPairAndMarkBothForDeletion(i, type.Value);
            }
        }

        return DeleteMarkedTokens(tokens);
    }

    private static Token? FindTokenPair(int tokenIndex, List<Token> tokens)
    {
        if (tokenIndex < 0 || tokenIndex >= tokens.Count)
            throw new IndexOutOfRangeException("Provided tokenIndex was out of range.");
        if (!tokens[tokenIndex].Type.SupportsClosingTag)
            throw new ArgumentException("Provided token is not a pair token.");

        Token? pair = null;
        if (tokens[tokenIndex].IsClosingTag && tokenIndex != 0)
        {
            for (var i = 0; i < tokenIndex; i++)
            {
                if (!tokens[i].IsClosingTag &&
                    TokenTypeEqualityComparer.Equals(tokens[i].Type, tokens[tokenIndex].Type))
                    pair = tokens[i];
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

    private static bool IsPrecededBySymbol(Token token, string line, Func<char, bool> condition)
        => token.StartingIndex > 0 && condition(line[token.StartingIndex - 1]);

    //удаляет пару открывающихся/закрывающихся тегов, если она находится внутри пары открывающихся/закрывающихся тегов другого типа
    private static List<Token> FilterNested(List<Token> tokens, ITokenType inner, ITokenType outer)
    {
        for (var i = 0; i < tokens.Count - 3; i++)
        {
            if (!ViolateNestingRules(tokens[i], tokens[i + 1], tokens[i + 2], tokens[i + 3], inner, outer))
                continue;
            tokens[i + 1].IsMarkedForDeletion = true;
            tokens[i + 2].IsMarkedForDeletion = true;
        }

        return DeleteMarkedTokens(tokens);
    }

    private static bool ViolateNestingRules(Token token1, Token token2, Token token3, Token token4, ITokenType inner,
        ITokenType outer)
        => TokenTypeEqualityComparer.Equals(token1.Type, outer)
           && TokenTypeEqualityComparer.Equals(token2.Type, inner)
           && TokenTypeEqualityComparer.Equals(token3.Type, inner)
           && TokenTypeEqualityComparer.Equals(token4.Type, outer)
           && !token1.IsClosingTag
           && !token2.IsClosingTag
           && token3.IsClosingTag
           && token4.IsClosingTag;

    private static bool IsTokenSurroundedWith(Token token, string str, Func<char, bool> condition, bool negateCondition)
    {
        var left = IsPrecededBySymbol(token, str, condition) ^ negateCondition;
        var right = IsFollowedBySymbol(token, str, condition) ^ negateCondition;

        return left && right;
    }

    private static bool IsInWord(Token token, string line)
    {
        if (IsTokenFirstOrLastInString(token, line.Length))
            return false;
        return !char.IsWhiteSpace(line[token.StartingIndex - 1]) &&
               !char.IsWhiteSpace(line[token.StartingIndex + token.Length]);
    }

    private static bool AreInDifferentWords(Token first, Token second, string line)
        => first is not null
           && second.IsClosingTag
           && (IsInWord(second, line)
               || IsInWord(first, line))
           && HasSymbolInBetween(line, ' ', first.StartingIndex, second.StartingIndex);

    private static Dictionary<string, List<Token>> CreatePairedTypesDictionary(List<Token> tokens)
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

    //удаляет сразу всю пару открывающийся/закрывающийся тег, если хотя бы один из них находится полностью в другом слове
    private static List<Token> FilterDifferentWords(List<Token> tokens, string line)
    {
        var types = CreatePairedTypesDictionary(tokens);

        foreach (var type in types)
        {
            Token? opening = null;
            foreach (var token in type.Value)
            {
                if (AreInDifferentWords(opening!, token, line))
                {
                    token.IsMarkedForDeletion = true;
                    opening!.IsMarkedForDeletion = true;
                    opening = null;
                    continue;
                }

                opening = token.IsClosingTag ? null : token;
            }
        }

        return DeleteMarkedTokens(tokens);
    }

    private static bool HasSymbolInBetween(string line, char symbol, int start, int end)
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

    private static List<Token> DeleteMarkedTokens(IEnumerable<Token> tokens)
        => tokens
            .Where(t => !t.IsMarkedForDeletion)
            .ToList();

    //если токен находится между цифрами, удаляем его и перераспределяем пары открывающихся/закрывающихся тегов
    private static List<Token> FilterBreakingNumbers(List<Token> tokens, string line)
    {
        foreach (var token in tokens.Where(currentToken => currentToken.Type.SupportsClosingTag))
            if (IsTokenSurroundedWith(token, line, char.IsDigit, false))
                token.IsMarkedForDeletion = true;

        var result = DeleteMarkedTokens(tokens);
        ReassignTagPairs(result);
        return result;
    }

    private static void ReassignTagPairs(List<Token> tokens)
    {
        var types = CreatePairedTypesDictionary(tokens);

        foreach (var type in types)
        {
            var isClosingTag = false;
            foreach (var token in type.Value)
            {
                token.IsClosingTag = isClosingTag;
                isClosingTag = !isClosingTag;
            }
        }
    }

    private static bool TokensEncloseEmptyLine(Token first, Token second)
        => first is not null
           && second is not null
           && first.Type.Value == second.Type.Value
           && !first.IsClosingTag && second.IsClosingTag
           && first.StartingIndex + first.Length == second.StartingIndex;

    //удаляет пару открывающийся/закрывающийся тег, если между ними пустая строка
    private static List<Token> FilterEmptyLines(List<Token> tokens)
    {
        Token? lastValidToken = null;
        foreach (var currentToken in tokens.Where(currentToken => currentToken.Type.SupportsClosingTag))
        {
            if (TokensEncloseEmptyLine(lastValidToken!, currentToken))
            {
                lastValidToken!.IsMarkedForDeletion = true;
                currentToken.IsMarkedForDeletion = true;
                lastValidToken = null;
                continue;
            }

            lastValidToken = currentToken;
        }

        return DeleteMarkedTokens(tokens);
    }
}
using Markdown.Tokens.Decorators;

namespace Markdown.Tokens.Utils;

public static class FilteringUtils
{
    public static List<TokenFilteringDecorator> DeleteMarkedTokens(IEnumerable<TokenFilteringDecorator> tokens)
        => tokens
            .Where(t => !t.IsMarkedForDeletion)
            .ToList();
    
    public static void FindPairAndMarkBothForDeletion(int tokenIndex, List<TokenFilteringDecorator> tokens)
    {
        if (tokenIndex < 0 || tokenIndex >= tokens.Count)
            throw new IndexOutOfRangeException("Provided tokenIndex was out of range.");

        tokens[tokenIndex].IsMarkedForDeletion = true;
        var pair = TokenUtils.FindTokenPair(tokenIndex, tokens);
        if (pair is not null)
            pair.IsMarkedForDeletion = true;
    }
    
    public static Dictionary<string, List<TokenFilteringDecorator>> CreatePairedTypesDictionary(IEnumerable<TokenFilteringDecorator> tokens)
    {
        var types = new Dictionary<string, List<TokenFilteringDecorator>>();

        foreach (var token in tokens.Where(currentToken => currentToken.Type.SupportsClosingTag))
        {
            if (!types.ContainsKey(token.Type.Value))
                types.Add(token.Type.Value, new List<TokenFilteringDecorator>());
            types[token.Type.Value].Add(token);
        }

        return types;
    }
    
    public static List<TokenFilteringDecorator> GetPairedTokens(IEnumerable<TokenFilteringDecorator> tokens)
        => tokens
            .Where(currentToken => currentToken.Type.SupportsClosingTag)
            .ToList();
}
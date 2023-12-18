using Markdown.Tokens;
using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//удаляет пару открывающийся/закрывающийся тег, если между ними пустая строка
public class BreakingNumbersFilter : TokenFilterChain
{
    private static List<TokenFilteringDecorator> ReassignTagPairs(List<TokenFilteringDecorator> tokens)
    {
        var tokenToPosition = tokens
            .Select((t, i) => new {Index = i, Item = t})
            .ToDictionary(p => p.Item, p => p.Index);
        
        var types = FilteringUtils.CreatePairedTypesDictionary(tokens);
        var reassignedPairs = new TokenFilteringDecorator[tokens.Count];
        
        foreach (var (token, _) in tokenToPosition.Where(t => !t.Key.Type.SupportsClosingTag))
            reassignedPairs[tokenToPosition[token]] = token;
        
        foreach (var (_, type) in types)
        {
            var isClosingTag = false;
            foreach (var token in type)
            {
                reassignedPairs[tokenToPosition[token]] = new TokenFilteringDecorator(
                    new Token(token.Type, isClosingTag, token.StartingIndex, token.Length));
                isClosingTag = !isClosingTag;
            }
        }
        
        return reassignedPairs.ToList();
    }
    
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        foreach (var token in tokens.Where(currentToken => currentToken.Type.SupportsClosingTag))
            if (TokenUtils.IsTokenSurroundedWith(token, line, char.IsDigit, false))
                token.IsMarkedForDeletion = true;

        var result = ReassignTagPairs(FilteringUtils.DeleteMarkedTokens(tokens));
        return base.Handle(result, line);
    }
}
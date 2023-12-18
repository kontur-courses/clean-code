using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//удаляет пару открывающийся/закрывающийся тег, если между ними пустая строка
public class BreakingNumbersFilter : TokenFilterChain
{
    private static void ReassignTagPairs(List<TokenFilteringDecorator> tokens)
    {
        var types = FilteringUtils.CreatePairedTypesDictionary(tokens);

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
    
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        foreach (var token in tokens.Where(currentToken => currentToken.Type.SupportsClosingTag))
            if (TokenUtils.IsTokenSurroundedWith(token, line, char.IsDigit, false))
                token.IsMarkedForDeletion = true;

        var result = FilteringUtils.DeleteMarkedTokens(tokens);
        ReassignTagPairs(result);
        return base.Handle(FilteringUtils.DeleteMarkedTokens(tokens), line);
    }
}
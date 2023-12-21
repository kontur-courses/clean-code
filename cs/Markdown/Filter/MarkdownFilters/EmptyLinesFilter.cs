using Markdown.Filter.Utils;
using Markdown.Tokens;
using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

public class EmptyLinesFilter : TokenFilterChain
{
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        TokenFilteringDecorator? lastValidToken = null;
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

        return base.Handle(FilteringUtils.DeleteMarkedTokens(tokens), line);
    }
    
    private static bool TokensEncloseEmptyLine(Token first, Token second)
        => first is not null
           && second is not null
           && first.Type.Value == second.Type.Value
           && !first.IsClosingTag && second.IsClosingTag
           && first.StartingIndex + first.Length == second.StartingIndex;
}
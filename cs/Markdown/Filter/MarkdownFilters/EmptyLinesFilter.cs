using Markdown.Tokens;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

public class EmptyLinesFilter : TokenFilterChain
{
    private static bool TokensEncloseEmptyLine(Token first, Token second)
        => first is not null
           && second is not null
           && first.Type.Value == second.Type.Value
           && !first.IsClosingTag && second.IsClosingTag
           && first.StartingIndex + first.Length == second.StartingIndex;
    
    public override List<Token> Handle(List<Token> tokens, string line)
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

        return base.Handle(TokenUtils.DeleteMarkedTokens(tokens), line);
    }
}
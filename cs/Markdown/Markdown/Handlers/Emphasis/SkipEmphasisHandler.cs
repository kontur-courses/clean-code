using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class SkipEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        if (tokens.Count < 1) return tokens;

        var handledTokens = new List<IToken>();
        IToken? previousToken = null;
        var token = tokens[0];

        for (var i = 1; i < tokens.Count; i++)
        {
            var nextToken = tokens[i];

            handledTokens.Add(IsNeedToSkipEmphasis(previousToken, token, nextToken)
                ? MarkdownTokenCreator.CreateTextToken(token.Content)
                : token);

            previousToken = token;
            token = nextToken;
        }

        handledTokens.Add(token);

        return handledTokens;
    }

    private static bool IsNeedToSkipEmphasis(IToken? previousToken, IToken token, IToken nextToken)
    {
        var isNeedToSkip = token.IsCloseTag switch
        {
            true => previousToken is { Type: TokenType.Space },
            false => nextToken.Type is TokenType.Space
        };
        return IsEmphasisTag(token) && isNeedToSkip;
    }
}
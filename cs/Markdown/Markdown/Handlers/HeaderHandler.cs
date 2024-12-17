using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

internal class HeaderHandler : ITokenHandler
{
    public IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        if (tokens.Count < 1) return tokens;

        var firstToken = tokens[0];

        return IsHeaderTag(firstToken)
            ? HandleTokensListWithHeaderTag(tokens)
            : HandleTokensListWithoutHeaderTag(tokens);
    }

    private static List<IToken> HandleTokensListWithHeaderTag(IReadOnlyList<IToken> tokens)
    {
        var openHeaderTag = tokens[0];
        var handledTokens = new List<IToken>(tokens)
            { MarkdownTokenCreator.CreateCloseTag(openHeaderTag, openHeaderTag.TagType, openHeaderTag) };
        return handledTokens;
    }

    private static List<IToken> HandleTokensListWithoutHeaderTag(IReadOnlyList<IToken> tokens) =>
        tokens.Select(token => IsHeaderTag(token) ? MarkdownTokenCreator.CreateTextToken(token.Content) : token)
            .ToList();

    private static bool IsHeaderTag(IToken token) => token.Type is TokenType.Tag && token.TagType is TagType.Header;
}
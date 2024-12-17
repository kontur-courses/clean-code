using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public abstract class EmphasisHandlerBase : ITokenHandler
{
    public abstract IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens);

    protected static bool IsEmphasisTag(IToken token) =>
        token.TagType is TagType.Italic or TagType.Strong;
    
    protected static List<IToken> RewriteTagsToText(IReadOnlyList<IToken> tokens, HashSet<int> tagIndex)
    {
        var handledTokens = new List<IToken>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            handledTokens.Add(tagIndex.Contains(i) ? MarkdownTokenCreator.CreateTextToken(token.Content) : tokens[i]);
        }

        return handledTokens;
    }
}
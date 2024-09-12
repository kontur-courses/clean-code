using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Filters;

public class StrongTagsFilter : IFilter
{
    private readonly string text;

    public StrongTagsFilter(string text)
    {
        this.text = text;
    }

    public int Order { get; } = 3;

    public IList<IToken> Filter(IList<IToken> tokens)
    {
        (IToken? Token, Tag? Tag) previousTokenAndTag = (null, null);
        IToken? prePreviousTag = null;
        var resultTokens = new List<IToken>();
        foreach (var token in tokens)
        {
            var currentTokenAndTag = TokenUtilities.GetTokenAndTag(token);
            if (currentTokenAndTag.Token.Type == TokenType.Tag
                && previousTokenAndTag.Token?.Type == TokenType.Tag
                && currentTokenAndTag.Tag?.TagType == TagType.Italic
                && previousTokenAndTag.Tag?.TagType == TagType.Italic
                && currentTokenAndTag.Tag.TagType != TagType.Strong
                && !TokenUtilities.IsPreviousTokenCloseAndPrePreviousOpen(previousTokenAndTag.Token, prePreviousTag, text))
            {

                currentTokenAndTag.Token.Content += previousTokenAndTag.Token.Content;
                currentTokenAndTag.Token.StartPosition = previousTokenAndTag.Token.StartPosition;
                currentTokenAndTag.Tag = SupportedTags.Tags[currentTokenAndTag.Token.Content];
                resultTokens.Remove(previousTokenAndTag.Token);
            }
            resultTokens.Add(currentTokenAndTag.Token);
            if (previousTokenAndTag.Token?.Type != TokenType.Text)
                prePreviousTag = previousTokenAndTag.Token;
            previousTokenAndTag = currentTokenAndTag;
        }
        return resultTokens;
    }
}

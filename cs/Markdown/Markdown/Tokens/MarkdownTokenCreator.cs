using Markdown.Html.Tags;
using Markdown.Markdown.Attributes;
using Markdown.Markdown.Tags;

namespace Markdown.Markdown.Tokens;

internal static class MarkdownTokenCreator
{
    public static IToken NewLine =>
        new MarkdownToken(MarkdownConstants.Newline, TokenType.NewLine);

    public static IToken CreateOpenTag(IToken token, TagType tagType, IToken? pair = null) =>
        new MarkdownToken(token.Content, TokenType.Tag, TagType: tagType, TagPair: pair);

    public static IToken CreateCloseTag(IToken token, TagType tagType, IToken? pair = null) =>
        new MarkdownToken(token.Content, token.Type, TagPair: pair, TagType: tagType, IsCloseTag: true);

    public static IToken CreateTagWithAttributes(IToken token, HashSet<IAttribute> attributes) =>
        new MarkdownToken(token.Content, token.Type, token.TagType, token.TagPair,
            attributes.ToDictionary(x => x.Type, x => x), token.IsCloseTag);

    public static IToken CreateTextToken(string content) =>
        new MarkdownToken(content, TokenType.Text);

    public static bool TryCreateSymbolToken(string content, out IToken token)
    {
        if (int.TryParse(content, out _))
        {
            token = new MarkdownToken(content, TokenType.Digit);
            return true;
        }

        token = content switch
        {
            MarkdownConstants.Space => new MarkdownToken(content, TokenType.Space),
            MarkdownConstants.Escape => new MarkdownToken(content, TokenType.Escape),
            _ => null!
        };

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return token is not null;
    }

    public static bool TryCreateTagToken(string content, out IToken token)
    {
        token = null!;
        if (!MarkdownTagValidator.Validate(content))
            return false;

        token = new MarkdownToken(content, TokenType.Tag);
        return true;
    }
}
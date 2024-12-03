using Markdown.TagInfo;
using Markdown.TokenInfo;
using Markdown.HtmlTools;

namespace Markdown.MarkdownParsers;

internal class EscapedCharactersParser : IMarkdownParser
{
    private const int TagLength = 1;
    private const Tag Tag = TagInfo.Tag.Empty;
    private readonly char[] escapedChars = ['_', '#', '\\'];

    public IEnumerable<Token> Parse(string text)
    {
        var tokens = new List<Token>();
        tokens.AddRange(FindEscapedTokens(text));

        return tokens.OrderBy(x => x.Position).ToList();
    }

    private IEnumerable<Token> FindEscapedTokens(string text)
    {
        var escapedCharPosition = text.IndexOfAny(escapedChars);

        while (escapedCharPosition >= 0)
        {
            if (Checks.IsEscaped(text, escapedCharPosition))
            {
                yield return BuildEscapeTag(escapedCharPosition);
            }

            escapedCharPosition = text.IndexOfAny(escapedChars, escapedCharPosition + 1);
        }
    }

    private static Token BuildEscapeTag(int escapedCharPosition)
    {
        return new Token(Tag, escapedCharPosition - 1, TagType.Open, TagLength);
    }
}
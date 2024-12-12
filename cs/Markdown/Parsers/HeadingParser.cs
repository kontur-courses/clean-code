using Markdown.TagsType;

namespace Markdown.Parsers;

public class HeadingParser : IParser
{
    public bool TryParse(char symbol, string text, int index)
    {
        return symbol == '#';
    }

    public Token Parse(string text, ref int index)
    {
        var headingTag = new HeadingTag();
        return new Token(headingTag.GetHtmlOpenTag, headingTag);
    }
}
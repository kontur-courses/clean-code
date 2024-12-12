using Markdown.TagsType;

namespace Markdown.Parsers;

public class SlashParser : IParser
{
    private int index;
    private BoldParser boldParser;

    public SlashParser(BoldParser boldParser)
    {
        this.boldParser = boldParser;
        index = 0;
    }

    private Token QuotedToken(string text, int i, bool isNextSymbolBold)
    {
        i++;
        index = i;
        if (index < text.Length)
        {
            switch (text[index])
            {
                case '_':
                    if (index + 1 < text.Length && isNextSymbolBold)
                    {
                        index++;
                        return new Token(new BoldTag().MarkdownTag, new TextTag());
                    }

                    return new Token(new ItalicTag().MarkdownTag, new TextTag());
                case '#':
                    return new Token(new HeadingTag().MarkdownTag, new TextTag());
            }
        }

        index--;
        return new Token("\\", new TextTag());
    }

    public int GetIndex() => index;

    public bool TryParse(char symbol, string text, int i)
    {
        return symbol == '\\';
    }

    public Token Parse(string text, ref int i)
    {
        var token = QuotedToken(text, i, boldParser.IsNextSymbolBold(text, i + 1));
        i = index;
        return token;
    }
}
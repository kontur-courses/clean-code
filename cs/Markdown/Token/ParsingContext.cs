using Markdown.Parser.TokenHandler;

namespace Markdown.Token;

public class ParsingContext
{
    public string Text { get; }
    public int Position { get; }
    public Stack<TokenType> OpenTags { get; }

    public ParsingContext(string text, int position, Stack<TokenType>? openTags)
    {
        Text = text;
        Position = position;
        OpenTags = openTags ?? new Stack<TokenType>();
    }

    public bool IsStartOfLine => Position == 0 || (Position > 0 && Text[Position - 1] == '\n');
}
using Markdown.Parser.TokenHandler;

namespace Markdown.Token;

public class Token
{
    public string Text { get; }
    public TokenType Type { get; }
    public TagState State { get; }
    public int Position { get; }
    public int Level { get; }
    public string Url { get; }

    public Token(string text, TokenType type, TagState state, int position, int level = 0, string url = null)
    {
        Text = text;
        Type = type;
        State = state;
        Position = position;
        Level = level;
        Url = url;
    }

    public static Token CreateText(string text, int position)
        => new(text, TokenType.Text, TagState.Open, position);

    public static Token CreateStrong(bool isOpening, int position)
        => new("__", TokenType.Strong, isOpening ? TagState.Open : TagState.Close, position);

    public static Token CreateItalic(bool isOpening, int position)
        => new("_", TokenType.Italic, isOpening ? TagState.Open : TagState.Close, position);

    public static Token CreateHeader(int level, int position)
        => new(new string('#', level), TokenType.Header, TagState.Open, position, level);

    public static Token CreateLink(string text, string url, int position)
        => new(text, TokenType.Link, TagState.Open, position, url: url);
    
}
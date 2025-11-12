using Markdown.Tokens;
using Markdown.Tokens.Tags;

namespace Markdown.Parsers;

public class EscapeParser : IParser
{
    public bool CanParse(char symbol, string text, int index)
    {
        return symbol == '\\';
    }

    public Token Parse(string text, int index)
    {
        if (index + 1 >= text.Length)
            return GetSimpleTextToken(index);

        var nextChar = text[index + 1];
        
        if (nextChar == '_' && index + 2 < text.Length && text[index + 2] == '_')
            return new Token(new TextTag(), "__", index + 2);

        if (nextChar is '_' or '#' or '\\' or '*' or '-' or '+')
            return new Token(new TextTag(), nextChar.ToString(), index + 1);

        return GetSimpleTextToken(index);
    }

    private Token GetSimpleTextToken(int index)
    {
        return new Token(new TextTag(), "\\", index);
    }
}
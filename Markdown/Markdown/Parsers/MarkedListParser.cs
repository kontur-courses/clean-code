using Markdown.Tokens;
using Markdown.Tokens.Tags;

namespace Markdown.Parsers;

public class MarkedListParser(char marker) : IParser
{
    public bool CanParse(char symbol, string text, int index)
    {
        if (symbol != marker)
            return false;

        if (index != 0)
            return false;

        if (text.Length > 1 && !char.IsWhiteSpace(text[1]))
            return false;
        
        return true;
    }

    public Token Parse(string text, int index)
    {
        var inner = text.Length > 2 ? text.Substring(2) : string.Empty;

        ITag tag = marker switch
        {
            '*' => new AsteriskTag(),
            '-' => new DashTag(),
            '+' => new PlusTag(),
            _   => new TextTag()
        };
        
        return new Token(tag, inner, text.Length - 1);
    }
}
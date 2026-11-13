using System.Text;
using Markdown.Models;

namespace Markdown.Abstractions;

public abstract class TagParser(string text, StringBuilder buffer, List<Token> tokens)
{
    public abstract bool TryParse(ref int currentPosition);
    
    protected internal void FlushText()
    {
        if (buffer.Length > 0)
            tokens.Add(new Token(TokenKind.Text, buffer.ToString()));
        buffer.Clear();
    }
}
using System.Collections.Generic;

namespace MarkdownProcessing.Tokens
{
    public enum TokenType
    {
        Parent,
        PlainText,
        Bold,
        Italic
    }

    public abstract class Token
    {
        public TokenType Type;
    }
}
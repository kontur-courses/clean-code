using System;

namespace Markdown
{
    public class Token
    {
        public string Content { get; }
        public TokenType Type { get; }
        public int Index { get; }

        public Token(string content, TokenType type, int index)
        {
            Content = content;
            Type = type;
            Index = index;
        }

        public string GetHtml()
        {
            throw new NotImplementedException();
        }
    }
}

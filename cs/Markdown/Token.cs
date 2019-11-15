using System;

namespace Markdown
{
    public class Token
    {
        public string Content { get; }
        public TokenType Type { get; }

        public Token(string content, TokenType type)
        {
            Content = content;
            Type = type;
        }

        public string GetHtml()
        {
            throw new NotImplementedException();
        }
    }
}

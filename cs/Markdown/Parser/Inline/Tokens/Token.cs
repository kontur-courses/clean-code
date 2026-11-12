using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Tokens
{
    public class Token
    {
        public TokenType Type { get; private set; }
        public string Value { get; private set;  }
        public string Url { get; private set; }
        public int Position { get; private set; }
        public bool CanOpen { get; set; }
        public bool CanClose { get; set; }
        public bool IsInsideWord { get; set; }
        public bool IsDisabledFromIntersection { get; set; }

        public Token(TokenType type, string value, int position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public void SetUrl(string url)
        {
            if (Type == TokenType.Link)
                Url = url;
            else
                throw new InvalidOperationException("Token type must be TokenType.Link");
        }
    }
}

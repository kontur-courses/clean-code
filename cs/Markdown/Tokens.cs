using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class Token
    {
        public virtual TokenType Type { get; set; }
        public string Text { get; set; }

        public static Token EmptyText => new Token() { Type = TokenType.Text, Text = string.Empty };
    }

    internal class ObjectOpenToken : Token
    {
        public override TokenType Type => TokenType.Object;
        public TokenObjectType ObjectType { get; set; }
    }

    internal class ObjectCloseToken : Token
    {
        public override TokenType Type => TokenType.Object;
        public TokenObjectType ObjectType { get; set; }
    }
}

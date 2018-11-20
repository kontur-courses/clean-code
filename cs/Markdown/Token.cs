using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public int Position;
        public int Length;
        public ITokenType Type;
        public bool Closed;
        public bool Opened;

        public string ToHtml(string mdText)
        {
            var text = GetText(mdText);
            return Type == null ? text : Type.ToHtml(text, Opened, Closed);
        }

        public string GetText(string mdText) => mdText.Substring(Position, Length);

        public Token Concat(Token another)
        {
            Position = Math.Min(Position, another.Position);
            var endPosition = Math.Max(Position + Length, another.Position + another.Length);
            Length = endPosition - Position;
            Closed = Closed || another.Closed;
            return this;
        }
    }
}

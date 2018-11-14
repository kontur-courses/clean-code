using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Token
    {
        public int Position;
        public int Length;
        //public string Text;
        public ITokenType Type;
        public List<Token> InnerTokens = new List<Token>();

        public string ToHtml(string mdText)
        {
            var text = GetText(mdText);
            return Type == null ? text : Type.ToHtml(text);
        }

        public string GetText(string mdText) => mdText.Substring(Position, Length);

        public void Concat(Token another)
        {
            Position = Math.Min(Position, another.Position);
            var endPosition = Math.Max(Position + Length, another.Position + another.Length);
            Length = endPosition - Position;
            InnerTokens.AddRange(another.InnerTokens);
        }
    }
}

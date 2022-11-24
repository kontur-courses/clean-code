using System.Collections.Generic;

namespace Markdown.Tags
{
    public class Title : Tag
    {
        public Title(Md md) : base(md, "#", new HashSet<char>())
        {
        }

        protected override string FormatTag(Token start, Token end, string strBetween)
        {
            if (start.Next?.Type != TokenType.Space)
                return $"{start.Value}{strBetween}";
            return $"<h1>{strBetween[1..]}</h1>" + (end?.Type == TokenType.BreakLine ? "\n" : "");
        }

        protected override Token FindEnd(Token start)
        {
            var current = start.Next;
            while (current != null && current.Type != TokenType.BreakLine)
                current = current.Next;
            return current;
        }
    }
}
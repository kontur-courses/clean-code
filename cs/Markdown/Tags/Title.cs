using System;

namespace Markdown.Tags
{
    public class Title : SimpleTag
    {
        public Title(Md md, int titleLevel)
            : base(md, string.Empty.PadLeft(titleLevel, '#'))
        {
            if (titleLevel < 1 || titleLevel > 6)
                throw new ArgumentException("Title level should be in range [1, 6]");
        }

        public int TitleLevel => Identifier.Length;

        protected override string FormatTag(Token start, Token end, string contains)
        {
            if (start.Next.Type != TokenType.Space)
                return $"{start.Value}{contains}";
            return $"<h{TitleLevel}>{contains.Substring(1)}</h{TitleLevel}>{end?.Value}";
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

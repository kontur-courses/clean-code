using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tags
{
    public class Title : Tag
    {
        public int TitleLevel => Identifier.Length;

        public Title(Md md, int titleLevel)
            : base(md, string.Empty.PadLeft(titleLevel, '#'), true)
        {
            if (titleLevel < 1 || titleLevel > 6)
                throw new ArgumentException("Title level should be in range [1, 6]");
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            if (start.Next.Type != TokenType.Space)
                return $"{start.Value}{contains}";
            return $"<h{TitleLevel}>{contains.Substring(1)}</h{TitleLevel}>{end?.Value}";
        }
    }
}

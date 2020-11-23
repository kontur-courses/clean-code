using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tags
{
    public abstract class SimpleTag : Tag
    {
        protected SimpleTag(Md md, string identifier) : base(md, identifier)
        {
        }

        public override string Format(Token start, out Token next)
        {
            var builder = new StringBuilder();
            next = start.Next;
            var end = FindEnd(start);
            while (next != null)
            {
                if (next == end)
                    break;
                builder.Append(Markdown.FormatToken(ref next));
            }

            if (start.Type == TokenType.Tag)
                return FormatTag(start, end, builder.ToString());
            return $"{start.Value}{builder}{next.Value}";
        }
    }
}

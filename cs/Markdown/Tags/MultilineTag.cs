using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Tags
{
    public abstract class MultilineTag : Tag
    {
        protected MultilineTag(Md md, string identifier) : base(md, identifier)
        {
        }

        public override string Format(Token start, out Token next)
        {
            var lines = new List<string>();
            var lineContent = new StringBuilder();
            var end = FindEnd(start);
            next = start;

            while (next != null && next != end)
            {
                if (next.Type == TokenType.BreakLine)
                {
                    lines.Add(FormatTag(start, next, lineContent.ToString()));
                    lineContent.Clear();
                    next = start = next.Next;
                    continue;
                }

                if (!EqualsIdentifier(next))
                    lineContent.Append(Markdown.FormatToken(ref next));
                else
                {
                    lineContent.Append(next.Value);
                    next = next.Next;
                }
            }
            if (lineContent.Length > 0)
                lines.Add(FormatTag(start, next, lineContent.ToString()));

            return PrepareResultLines(lines);
        }

        protected virtual string PrepareResultLines(List<string> lines)
        {
            return string.Join("", lines);
        }
    }
}

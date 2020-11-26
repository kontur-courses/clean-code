using System.Collections.Generic;
using System.Text;

namespace Markdown.Tags
{
    public abstract class MultilineTag : Tag
    {
        protected MultilineTag(Md md, string identifier) : base(md, identifier)
        {
        }

        public override string Format(Token start, out Token current)
        {
            var lines = new List<string>();
            var lineContent = new StringBuilder();
            var end = FindEnd(start);
            current = start;

            while (current != null && current != end)
            {
                if (current.Type == TokenType.BreakLine)
                {
                    lines.Add(FormatTag(start, current, lineContent.ToString()));
                    lineContent.Clear();
                    current = start = current.Next;
                    continue;
                }

                if (!EqualsIdentifier(current))
                {
                    lineContent.Append(Markdown.FormatToken(ref current));
                }
                else
                {
                    lineContent.Append(current.Value);
                    current = current.Next;
                }
            }

            if (lineContent.Length > 0)
                lines.Add(FormatTag(start, current, lineContent.ToString()));

            return PrepareResultLines(lines);
        }

        protected override Token FindNext(Token token, TokenType target)
        {
            while (token != null && token.Type != target)
                token = token.Next;
            return token;
        }

        protected virtual string PrepareResultLines(List<string> lines)
        {
            return string.Join("", lines);
        }
    }
}

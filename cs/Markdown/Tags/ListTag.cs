using System.Collections.Generic;

namespace Markdown.Tags
{
    public class ListTag : MultilineTag
    {
        public ListTag(Md md, string identifier) : base(md, identifier)
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            if (!IsStartLine(start))
                return contains;
            var endString = end == null ? string.Empty : end.Value;
            return $"<li>{contains.Substring(Identifier.Length)}</li>{endString}";
        }

        protected override Token FindEnd(Token start)
        {
            var current = start.NextLine;
            var foundedEnd = start;
            while (current != null)
            {
                if (!EqualsIdentifier(current) || current.Type == TokenType.Space)
                    break;
                foundedEnd = current;
                current = current.NextLine;
            }

            return FindNext(foundedEnd, TokenType.BreakLine);
        }

        protected override string PrepareResultLines(List<string> lines)
        {
            return $"<ul>{base.PrepareResultLines(lines)}</ul>";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tags
{
    public class ListTag : Tag
    {
        public ListTag(Md md, string identifier) : base(md, identifier)
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            var current = start;
            var builder = new StringBuilder();
            builder.Append("<ul>");
            while (current != null)
            {
                if (current.Type != TokenType.Tag && current.Value != Identifier)
                    break;
                current = current.Next;
                builder.Append(current.Value.Substring(1));
                current = current.Next;
                builder.Append("<li>");
                while (current != null && current.Type != TokenType.BreakLine)
                {
                    builder.Append(current.Value);
                    current = current.Next;
                }

                builder.Append("</li>");
                if (current == null)
                    break;
                builder.Append(current.Value);
                current = current.Next;
            }

            builder.Append("</ul>");
            return builder.ToString();
        }

        protected override Token FindEnd(Token start)
        {
            var current = start;
            var foundedEnd = start;
            //if (current.Previus.Type != TokenType.BreakLine)
            //    return start;
            while (current != null)
            {
                if (current.Type == TokenType.Tag && current.Value == Identifier)
                {
                    while (current != null && current.Type != TokenType.BreakLine)
                    {
                        foundedEnd = current;
                        current = current.Next;
                    }

                    current = current?.Next;
                    continue;
                }
                break;
            }
            return foundedEnd;
        }
    }
}

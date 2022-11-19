using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public abstract class Tag
    {
        protected readonly Md Markdown;
        public string Identifier { get; }

        protected Tag(Md md, string identifier)
        {
            Markdown = md;
            Identifier = identifier;
        }

        public virtual string Format(Token start, out Token next)
        {
            var builder = new StringBuilder();
            var current = start.Next;
            var end = FindEnd(start);
            next = start;
            if (start.IgnoreAsTag)
                return start.Value;
            while (current != null)
            {
                if (current == end)
                    break;
                builder.Append(Markdown.FormatToken(ref current));
            }

            next = current;
            if (start.Type == TokenType.Tag)
                return FormatTag(start, end, builder.ToString());
            return $"{start.Value}{builder}{current.Value}";
        }

        protected virtual Token FindEnd(Token start)
        {
            var identifiers = new Stack<Token>();
            var current = start.Next;
            while (current != null && current.Value != Identifier)
            {
                if (current.Type == TokenType.Tag)
                {
                    if (identifiers.Count > 0 && identifiers.Peek().Value == current.Value)
                        identifiers.Pop();
                    else
                        identifiers.Push(current);
                }

                current = current.Next;
            }

            if (identifiers.Count > 0)
                start.IgnoreTag();
            return current?.IgnoreTag();
        }

        protected virtual string FormatTag(Token start, Token end, string strBetween)
        {
            //TODO
            return strBetween;
        }
    }
}

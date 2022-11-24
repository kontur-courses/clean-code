using System.Collections.Generic;

namespace Markdown.Tags
{
    public class Link : Tag
    {
        private string text = "";
        private string url = "";

        public Link(Md md) : base(md, "[", new HashSet<char> { ']', '(', ')' })
        {
        }

        public override string Format(Token start, out Token next)
        {
            var current = start.Next;
            while (current != null && !(current.Type == TokenType.Tag && current.Value == "]"))
                if (current.Type == TokenType.Tag)
                {
                    text += Markdown.FormatToken(ref current);
                }
                else
                {
                    text += current.Value;
                    current = current?.Next;
                }

            while (current?.Type == TokenType.Tag && current.Next?.Value == "]")
            {
                text += current.Value;
                current = current.Next;
            }

            current = current?.Next;
            if (current?.Value != "(")
            {
                next = start.Next;
                return start.Value + start?.Next.Value;
            }

            current = current?.Next;
            while (current != null && !(current.Type == TokenType.Tag && current.Value == ")"))
            {
                url += current.Value;
                current = current.Next;
            }

            if (current?.Value != ")")
            {
                next = start.Next;
                return start.Value + start?.Next.Value;
            }

            while (current?.Type == TokenType.Tag && current.Next?.Value == ")")
            {
                text += current.Value;
                current = current.Next;
            }

            next = current;
            return $"<a href=\"{url}\">{text}</a>";
        }
    }
}
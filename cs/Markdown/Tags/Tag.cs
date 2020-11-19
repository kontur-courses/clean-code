using System.Text;

namespace Markdown.Tags
{
    public abstract class Tag
    {
        protected readonly Md Markdown;

        protected Tag(Md md, string identifier)
        {
            Markdown = md;
            Identifier = identifier;
        }

        public string Identifier { get; }

        public string Format(Token start, out Token next)
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

        protected virtual Token FindEnd(Token start)
        {
            var current = start.Next;
            while (current != null && current.Value != Identifier)
                current = current.Next;
            return current;
        }

        protected virtual string FormatTag(Token start, Token end, string contains)
        {
            return contains;
        }
    }
}

using System.Text;

namespace Markdown
{
    public abstract class Tag
    {
        private readonly bool dropClosingTag;
        protected readonly Md Markdown;

        protected Tag(Md md, string identifier, bool dropClosingTag = false)
        {
            Markdown = md;
            Identifier = identifier;
            this.dropClosingTag = dropClosingTag;
        }

        public virtual string Identifier { get; }

        public string Format(Token start, out Token next)
        {
            var builder = new StringBuilder();
            next = start.Next;
            while (next != null)
            {
                if (!dropClosingTag && next.Value == Identifier || next.Type == TokenType.BreakLine)
                    break;
                builder.Append(Markdown.FormatToken(ref next));
            }

            if (start.Type == TokenType.Tag)
                return FormatTag(start, next, builder.ToString());
            return $"{start.Value}{builder}{next.Value}";
        }


        protected virtual string FormatTag(Token start, Token end, string contains)
        {
            return contains;
        }
    }
}

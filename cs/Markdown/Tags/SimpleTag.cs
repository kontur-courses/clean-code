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

        protected override string FormatTag(Token start, Token end, string contains)
        {
            var inWordTag = IsInType(start, TokenType.Word);
            var current = start.Next;
            var typesInside = TokenType.Undefined;
            while (current != end)
            {
                typesInside |= current.Type;
                if (inWordTag && current.Type == TokenType.Space)
                    return WithoutFormat(start, end, contains);
                current = current.Next;
            }

            var requireForSkip = TokenType.Number;
            if ((typesInside & requireForSkip) == requireForSkip)
                return WithoutFormat(start, end, contains);
            return null;
        }

        protected string FormatSimpleTag(Token token, Token end)
        {
            var start = token;
            token = token.Next;
            while (token != null && token != end)
            {
                if (EqualsIdentifier(token, start.Value))
                    return Markdown.FormatToken(ref token);
                if (token.Type == TokenType.Tag)
                    return FormatSimpleTag(token, end);
                token = token.Next;
            }

            return null;
        }
    }
}

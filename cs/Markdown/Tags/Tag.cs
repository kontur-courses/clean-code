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

        public virtual string Format(Token start, out Token next)
        {
            next = start?.Next;
            return start?.Value;
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

        protected bool IsInType(Token token, TokenType type)
        {
            return token.Previous?.Type == type
                   && token.Next?.Type == type;
        }

        protected Token FindNext(Token token, TokenType target)
        {
            while (token != null && token.Type != target)
                token = token.Next;
            return token;
        }

        protected bool EqualsIdentifier(Token token)
        {
            return token.Type == TokenType.Tag && token.Value == Identifier;
        }

        protected bool IsStartLine(Token token)
        {
            return token.Previous == null || token.Line != token.Previous.Line;
        }
    }
}

using System.Collections.Generic;

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

        public virtual string Format(Token start, out Token current)
        {
            current = start?.Next;
            return start?.Value;
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
                start.TagIgnore();
            return current?.TagIgnore();
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

        protected virtual Token FindNext(Token token, TokenType target)
        {
            while (token != null && token.Type != target)
                token = token.NextSomeType;
            return token;
        }

        protected bool EqualsIdentifier(Token token)
        {
            return EqualsIdentifier(token, Identifier);
        }

        protected bool EqualsIdentifier(Token token, string identifier)
        {
            return token.Type == TokenType.Tag && token.Value == identifier;
        }

        protected bool IsStartLine(Token token)
        {
            return token.Previous == null || token.Line != token.Previous.Line;
        }

        protected string WithoutFormat(Token start, Token end, string contains)
        {
            return $"{start.Value}{contains}{end?.Value}";
        }
    }
}

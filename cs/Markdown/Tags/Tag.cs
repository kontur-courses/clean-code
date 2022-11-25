using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tags
{
    public abstract class Tag
    {
        public readonly Md Markdown;

        protected Tag(Md md, string identifier, HashSet<char> specialSymbols)
        {
            Markdown = md;
            Identifier = identifier;
            SpecialSymbols = specialSymbols;
        }

        public string Identifier { get; }

        public HashSet<char> SpecialSymbols { get; }

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
                builder.Append(Markdown.FormatTokenAndMoveToNext(ref current));
            }

            next = current;
            if (start.Type == TokenType.Tag)
                return FormatTag(start, end, builder.ToString());
            return $"{start.Value}{builder}{current.Value}";
        }

        protected virtual Token FindEnd(Token start)
        {
            var identifiers = new Stack<Token>();
            if (start.Next?.Type == TokenType.Space)
            {
                start.IgnoreTag();
                return start.Next;
            }

            var current = start.Next;

            while (current != null && (current.Value != start.Value
                                       || (current.Value == start.Value && current.Previous?.Type == TokenType.Space)))
            {
                if (current.Type == TokenType.Tag)
                {
                    if (current.Previous?.Type == TokenType.Space && start.Value == current.Value) current.IgnoreTag();

                    if (identifiers.Count > 0 && identifiers.Peek().Value == current.Value)
                        identifiers.Pop();
                    else
                        identifiers.Push(current);
                }

                current = current.Next;
            }

            while (identifiers.Count > 0)
            {
                if (identifiers.Peek().Value != start.Value)
                {
                    var tokenInside = identifiers.Peek();
                    Markdown.dfsTags.Add(start.Value);
                    var end = FindEnd(tokenInside);
                    if (end?.StartIndex > current?.StartIndex || Markdown.dfsTags.Contains(tokenInside.Value))
                        start.IgnoreTag();
                    else
                        tokenInside.IgnoreTag();
                    Markdown.dfsTags.Remove(start.Value);
                }

                identifiers.Pop();
            }

            return current?.IgnoreTag();
        }

        protected virtual string FormatTag(Token start, Token end, string strBetween)
        {
            var inWordTag = start.Previous?.Type == TokenType.Word
                            && start.Next?.Type == TokenType.Word;
            var typesBetween = TokenType.Undefined;
            for (var current = start.Next; current != null && current != end; current = current.Next)
            {
                typesBetween |= current.Type;
                if (inWordTag && current.Type == TokenType.Space)
                    return NoFormat(start, end, strBetween);
                if (current.Type == TokenType.Space && current.Next == end)
                    return NoFormat(start, end, strBetween);
            }

            var requireForSkip = TokenType.Number;
            if (typesBetween == TokenType.Undefined)
                return NoFormat(start, end, strBetween);
            if ((typesBetween & requireForSkip) == requireForSkip)
                return NoFormat(start, end, strBetween);
            throw new FormatException("format is not defined");
        }

        protected string NoFormat(Token start, Token end, string strBetween)
        {
            return $"{start?.Value}{strBetween}{end?.Value}";
        }
    }
}
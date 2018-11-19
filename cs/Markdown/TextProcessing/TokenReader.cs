using System;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TokenReader
    {
        public int Position { get; set; }
        public string Content { get; set; }
        public TextBuilder Builder { get; set; }

        public TokenReader(string content)
        {
            Content = content;
            Builder = new TextBuilder();
        }

        public IToken ReadToken(Func<char, bool> isStopChar, IToken iToken)
        {
            var startPosition = Position;
            var value = ReadWhile(isStopChar, iToken);
            var length = Position - startPosition;
            if (Position == Content.Length && !iToken.IsStopToken(Content, Position - 1))
            {
                value = iToken.TokenAssociation + value;
                return new SimpleToken(startPosition, length, value);
            }
            Position += iToken.TokenAssociation.Length == 0 ? 1 : iToken.TokenAssociation.Length;
            return CreateToken(iToken, startPosition, length, value);
        }

        private string ReadWhile(Func<char, bool> isStopChar, IToken iToken)
        {
            var value = "";
            while (Position < Content.Length && (!isStopChar(Content[Position]) || !iToken.IsStopToken(Content, Position)))
            {
                if (Position + 1 < Content.Length && isStopChar(Content[Position + 1]) && Content[Position] == '\\')
                {
                    Position++;
                    continue;
                }
                if (isStopChar(Content[Position]) && iToken.IsStartToken(Content, Position))
                {
                    value += iToken.TokenAssociation;
                    Position += iToken.TokenAssociation.Length;
                    continue;
                }
                if (isStopChar(Content[Position]) && iToken.IsNestedToken(Content, Position))
                {
                    Position++;
                    value += Builder.BuildToken(ReadToken(isStopChar, iToken.GetNextNestedToken(Content, Position)));
                    continue;
                }
                value += Content[Position];
                Position++;
            }

            return value;
        }

        public IToken CreateToken(IToken token, int startPosition, int length, string value)
        {
            if (token is EmToken)
                return new EmToken(startPosition, length, value);
            if (token is StrongToken)
                return new StrongToken(startPosition, length, value);
            return new SimpleToken(startPosition, length, value);
        }
    }
}
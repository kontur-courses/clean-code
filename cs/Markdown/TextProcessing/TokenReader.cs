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

        public Token ReadToken(Func<char, bool> isStopChar, ITokenHandler iToken)
        {
            var value = ReadWhile(isStopChar, iToken);
            if (Position == Content.Length && !iToken.IsStopToken(Content, Position - 1))
            {
                value = iToken.TokenAssociation + value;
                return new Token(TypeToken.Simple, value);
            }
            Position += iToken.TokenAssociation.Length == 0 ? 1 : iToken.TokenAssociation.Length;
            return CreateToken(iToken, value);
        }

        private string ReadWhile(Func<char, bool> isStopChar, ITokenHandler iToken)
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
                    value += Builder.BuildTokenValue(ReadToken(isStopChar, iToken.GetNextNestedToken(Content, Position)));
                    continue;
                }
                value += Content[Position];
                Position++;
            }

            return value;
        }

        public Token CreateToken(ITokenHandler token, string value)
        {
            if (token is EmTokenHandler)
                return new Token(TypeToken.Em, value);
            if (token is StrongTokenHandler)
                return new Token(TypeToken.Strong, value);
            return new Token(TypeToken.Simple, value);
        }
    }
}
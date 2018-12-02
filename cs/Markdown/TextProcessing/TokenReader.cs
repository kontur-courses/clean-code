using System;
using Markdown.TokenEssences;

namespace Markdown.TextProcessing
{
    public class TokenReader
    {
        public int Position { get; set; }
        public string Content { get; set; }
        public TextBuilder Builder { get; set; }

        public TokenReader(string content, TextBuilder builder)
        {
            Content = content;
            Builder = builder;
        }

        public IToken ReadToken( ITokenHandler tokenHandler)
        {
            var value = ReadWhile(ch => ch == tokenHandler.StopChar, tokenHandler);
            if (Position == Content.Length && !tokenHandler.IsStopToken(Content, Position - 1))
            {
                value = tokenHandler.TokenAssociation + value;
                return new Token(TypeToken.Simple, value);
            }
            Position += tokenHandler.TokenAssociation.Length == 0 ? 1 : tokenHandler.TokenAssociation.Length;
            return CreateToken(tokenHandler, value);
        }

        private string ReadWhile(Func<char, bool> isStopChar, ITokenHandler tokenHandler)
        {
            var value = "";
            while (Position < Content.Length && (!isStopChar(Content[Position]) || !tokenHandler.IsStopToken(Content, Position)))
            {
                if (Position + 1 < Content.Length && isStopChar(Content[Position + 1]) && Content[Position] == '\\')
                {
                    Position++;
                    continue;
                }
                if (isStopChar(Content[Position]) && tokenHandler.IsStartToken(Content, Position))
                {
                    value += tokenHandler.TokenAssociation;
                    Position += tokenHandler.TokenAssociation.Length;
                    continue;
                }
                if (isStopChar(Content[Position]) && tokenHandler.ContainsNestedToken(Content, Position))
                {
                    Position++;
                    value += Builder.BuildTokenValue(ReadToken(tokenHandler.GetNextNestedToken(Content, Position)));
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
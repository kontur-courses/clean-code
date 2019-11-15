using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdParser
    {
        private readonly string text;

        public MdParser(string text)
        {
            this.text = text;
        }

        public IEnumerable<Token> GetTokens()
        {
            var tokens = new List<Token>();
            var startPosition = 0;
            var currentPosition = 0;
            var tokenTagType = GetTagType(text, startPosition);

            while (currentPosition < text.Length)
            {
                var symbolType = GetTagType(text, currentPosition);
                if (IsTextToken(tokenTagType, symbolType) ||
                    IsEmToken(tokenTagType, symbolType, currentPosition, startPosition) ||
                    IsStrongToken(tokenTagType, symbolType, currentPosition, startPosition))
                {
                    currentPosition += symbolType is StrongTagType ? 2 : 1;
                }
                else
                {
                    tokens.Add(new Token(startPosition, currentPosition - startPosition, new DefaultTagType()));
                    startPosition = currentPosition;
                    tokenTagType = symbolType;
                }

                if (currentPosition == text.Length - 1)
                {
                    tokens.Add(new Token(startPosition, currentPosition - startPosition, tokenTagType));
                    currentPosition++;
                }
            }

            foreach (var token1 in tokens)
            {
                Console.WriteLine(token1.Position);
            }

            return tokens;
        }

        private TagType GetTagType(string text, int position)
        {
            if (EmTagType.IsOpenedTag(text, position) || EmTagType.IsClosedTag(text, position))
                return new EmTagType();
            if (StrongTagType.IsOpenedTag(text, position) || StrongTagType.IsClosedTag(text, position))
                return new StrongTagType();
            return new DefaultTagType();
        }

        private bool IsTextToken(TagType tokenTagType, TagType symbolType)
        {
            return tokenTagType is DefaultTagType && symbolType is DefaultTagType;
        }

        private bool IsEmToken(TagType tokenTagType, TagType symbolType, int currentPosition, int startPosition)
        {
            return tokenTagType is EmTagType && (!(symbolType is EmTagType) || currentPosition <= startPosition);
        }

        private bool IsStrongToken(TagType tokenTagType, TagType symbolType, int currentPosition, int startPosition)
        {
            return tokenTagType is StrongTagType &&
                   (!(symbolType is StrongTagType) || currentPosition <= startPosition);
        }
    }
}
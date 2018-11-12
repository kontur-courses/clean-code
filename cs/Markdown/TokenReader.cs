using System;

namespace Markdown
{
    public class TokenReader
    {
        public int Position { get; private set; }
        public readonly string MdText;

        public TokenReader(string mdText)
        {
            MdText = mdText;
        }

        public Token ReadUntil(Func<char, bool> isStopChar)
        {
            return ReadToken(c => !isStopChar(c));
        }

        public Token ReadWhile(Func<char, bool> accept)
        {
            return ReadToken(accept);
        }

        private Token ReadToken(Func<char, bool> condition)
        {
            var startPosition = Position;
            while (condition(MdText[Position]) && Position < MdText.Length)
                Position++;

            return new Token
            {
                Length = Position - startPosition,
                Position = Position
            };
        }
    }
}
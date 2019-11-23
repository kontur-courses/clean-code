using System;

namespace Markdown.BasicTextTokenizer
{
    public class TokenReader
    {
        public string Text { get; }
        public int Position { get; private set; }
        public bool HasData => Position < Text.Length;

        public TokenReader(string text)
        {
            Text = text;
        }

        public Token ReadUntil(Func<string, int, bool> isStopPosition)
        {
            var startPosition = Position;
            var length = 0;
            for (; Position < Text.Length; Position++)
            {
                if (isStopPosition(Text, Position))
                    return Token.CreateTextToken(startPosition, length);
                length++;
            }
            return Token.CreateTextToken(startPosition, length);
        }

        public Token ReadWhile(Func<char, bool> isAccepted)
        {
            var startPosition = Position;
            var length = 0;
            for (; Position < Text.Length; Position++)
            {
                if (!isAccepted(Text[Position]))
                    return Token.CreateTextToken(startPosition, length);
                length++;
            }
            return Token.CreateTextToken(startPosition, length);
        }

        public Token ReadCount(int count)
        {
            var startPosition = Position;
            var length = 0;
            for (var i = 0; i < count; i++)
            {
                Position++;
                length++;
                if (i >= Text.Length)
                    break;
            }
            return Token.CreateTextToken(startPosition, length);
        }

        public void SkipCount(int count)
        {
            Position += count;
        }
    }
}

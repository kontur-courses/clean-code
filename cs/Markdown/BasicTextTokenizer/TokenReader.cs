using System;
using System.Collections.Generic;

namespace Markdown.BasicTextTokenizer
{
    public class TokenReader
    {
        public string Text { get; }
        public int Position { get; private set; }

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
                    return new Token(startPosition, length);
                length++;
            }
            return new Token(startPosition, length);
        }

        public Token ReadWhile(Func<char, bool> isAccepted)
        {
            var startPosition = Position;
            var length = 0;
            for (; Position < Text.Length; Position++)
            {
                if (!isAccepted(Text[Position]))
                    return new Token(startPosition, length);
                length++;
            }
            return new Token(startPosition, length);
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
            return new Token(startPosition, length);
        }

        public List<Token> ReadUntilWithEscapeProcessing(
            Func<string, int, bool> isStopPosition, Func<string, int, bool> isEscapePosition)
        {
            bool IsStopPositionWithEscape(string text, int position) => isEscapePosition(text, position)
                                                                        || isStopPosition(text, position);
            var tokens = new List<Token>();
            var firstTime = true;
            do
            {
                Token escapedToken = null;
                if (!firstTime)
                {
                    Position++;
                    escapedToken = ReadCount(1);
                }
                var token = ReadUntil(IsStopPositionWithEscape);
                if (escapedToken != null)
                    token = escapedToken.Add(token);
                if (token.Length != 0)
                    tokens.Add(token);
                firstTime = false;
            } while (isEscapePosition(Text, Position));
            return tokens;
        }

        public bool HasData()
        {
            return Position < Text.Length;
        }
    }
}

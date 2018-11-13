using System;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TokenReader
    {
        public int Position { get; set; }
        public string Content { get; set; }

        public TokenReader(string content)
        {
            Content = content;
        }

        public Token ReadUntil(Func<char, bool> isStopChar, TypeToken typeToken)
        {
            var value = "";
            var length = 0;
            var startPosition = Position;
            while (IsNotEndOfToken(isStopChar))
            {
                if (Content[Position] == '\\')
                {
                    Position++;
                    continue;
                }
                length++;
                value += Content[Position];
                Position++;
            }
            Position++;
            return new Token(startPosition, length, value, typeToken);
        }

        private bool IsNotEndOfToken(Func<char, bool> isStopChar)
        {
            return Position < Content.Length && (!isStopChar(Content[Position]) || Position > 0 && Content[Position - 1] == '\\');
        }
    }
}
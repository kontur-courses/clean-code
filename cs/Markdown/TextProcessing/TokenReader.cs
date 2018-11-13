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
                if (Position + 1 < Content.Length && isStopChar(Content[Position + 1]) && Content[Position] == '\\')
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

        //Выглядит это ужасно, позже надо будет заменить.
        private bool IsNotEndOfToken(Func<char, bool> isStopChar)
        {
            var isEscapeCharacter = Position > 0 && Content[Position - 1] == '\\';
            var isNotStopChar = Position + 1 < Content.Length && Position - 1 >= 0 &&
                                isStopChar(Content[Position]) &&
                                char.IsLetterOrDigit(Content[Position - 1]) &&
                                char.IsLetterOrDigit(Content[Position + 1]);
            return Position < Content.Length && (!isStopChar(Content[Position]) || isEscapeCharacter || isNotStopChar);
        }
    }
}
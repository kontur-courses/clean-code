using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token : IToken
    {

        public TokenType Type;
        public TokenElement Element;
        public bool isText = false;
        public int Position { get; set; }
        public int Length { get; set; }
        public int End => Position + Length - 1;

        public Token(int position, int length)
        {
            Position = position;
            Length = length;
        }

        public Token(int position, int length, TokenType type)
        {
            Position = position;
            Length = length;
            Type = type;
        }

        public Token(int position, int length, TokenType type, TokenElement element)
        {
            Position = position;
            Length = length;
            Type = type;
            Element = element;
        }

        public override bool Equals(object obj)
        {
            if ((Token)obj == null)
                return false;
            return Equals((Token)obj);
        }

        public void ToDefault()
        {
            Type = TokenType.Default;
        }

        public int GetIndexNextToToken()
        {
            return Position + Length;
        }

        private static readonly List<char?> BlackListForOpen = new() { '\n', '\r' };
        private static readonly List<char?> BlackListForClose = new() { ' ', null };

        public TokenElement GetElementInText(string mdString)
        {
            char? charBeforeTag = Position == 0 ? null : mdString[Position - 1];
            char? charAfterTag = Position < mdString.Length - Length ? mdString[Position + Length] : null;
            if (BlackListForOpen.Contains(charAfterTag) || BlackListForClose.Contains(charAfterTag))
            {
                if (BlackListForClose.Contains(charBeforeTag))
                    return TokenElement.Default;
                return TokenElement.Close;
            }

            if (BlackListForClose.Contains(charBeforeTag))
                return TokenElement.Open;
            return TokenElement.Unknown;
        }
    }
}

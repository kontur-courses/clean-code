using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Markdown;

namespace Markdown.Tokens
{
    public class Token : IToken
    {

        public TokenType Type;
        public TokenElement Element;

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




        private static readonly List<char?> BlackListForClose = new() { ' ', null };
        private static readonly HashSet<char?> BlackListForOpen = new() { ' ', null, '\\', '\n', '\r' };

        public TokenElement GetElementInText(string mdString)
        {
            char? charBeforeTag = null;
            char? charAfterTag = null;
            if (Position >= 1)
                charBeforeTag = mdString[Position - 1];
            if (Position + Length < mdString.Length)
                charAfterTag = mdString[Position + Length];
            if (BlackListForOpen.Contains(charAfterTag))
            {
                return BlackListForClose.Contains(charBeforeTag)
                    ? TokenElement.Default
                    : TokenElement.Close;
            }
            return BlackListForClose.Contains(charBeforeTag)
                ? TokenElement.Open
                : TokenElement.Unknown;
        }
        public string CreateString(string md)
        {
            return md.Substring(Position, Length);

        }
    }
}

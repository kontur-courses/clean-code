using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tokens
{
    public sealed record UnderscoreToken : Token
    {
        public int Length { get; }
        public char? Left { get; }
        public char? Right { get; }
        public bool LeftEscaped { get; }
        public bool RightEscaped { get; }

        public UnderscoreToken(int length, char? left, char? right, bool leftEscaped, bool rightEscaped)
        {
            Length = length;
            Left = left;
            Right = right;
            LeftEscaped = leftEscaped;
            RightEscaped = rightEscaped;
        }

        public UnderscoreToken(int length, char? left, char? right)
        {
            Length = length;
            Left = left;
            Right = right;
            LeftEscaped = false;
            RightEscaped = false;
        }
    }
}

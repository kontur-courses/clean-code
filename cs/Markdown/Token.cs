using System;

namespace Markdown
{
    public class Token
    {
        public static readonly Token EmptyToken = new Token(0, "", TokenType.Empty);

        public int Position { get; }
        public string Text { get; }
        public int Length => Text.Length;
        public bool IsEmpty => Length == 0;
        public TokenType TokenType;

        public Token(int position, string text, TokenType tokenType = TokenType.Undefined)
        {
            Position = position;
            Text = text;
            TokenType = tokenType;
        }
    }
}
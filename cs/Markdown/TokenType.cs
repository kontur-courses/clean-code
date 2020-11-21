using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class TokenType
    {
    }

    public abstract class BasicTokenType : TokenType
    {
        public readonly string Start;
        public readonly string End;

        public readonly bool StartWithNewLine = false;
        public readonly bool EndWithNewLine = false;

        public readonly List<TokenType> DisallowedTokenTypes = new List<TokenType>();

        protected BasicTokenType(string start, string end)
        {
            StartWithNewLine = start.StartsWith("\n");
            EndWithNewLine = end.EndsWith("\n");

            if (StartWithNewLine) start = start.Substring(1);
            if (EndWithNewLine) end = end.Substring(0, end.Length - 1);

            Start = start;
            End = end;
        }

        public abstract BasicToken CreateInstance(int startPosition = 0, int length = 0, Token parent = null);
    }

    public class BasicTokenType<TToken> : BasicTokenType where TToken : BasicToken, new()
    {
        public BasicTokenType(string start, string end) : base(start, end)
        {
        }

        public override BasicToken CreateInstance(int startPosition = 0, int length = 0, Token parent = null)
            => new TToken {StartPosition = startPosition, Length = length, Parent = parent};
    }

    public class CustomTokenType : TokenType
    {
        public readonly Func<TokenReader, Token> ReadFunc;

        public CustomTokenType(Func<TokenReader, Token> readFunc)
        {
            ReadFunc = readFunc;
        }
    }
}
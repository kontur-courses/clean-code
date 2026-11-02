using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class InlineTokenizer
    {
        public static Token[] Tokenize(string text)
        {
            throw new NotImplementedException();
        }
    }

    public class Token { }

    public class TextToken : Token
    {
        public string Text { get; }
        public TextToken(string text) { Text = text; }
    }

    public sealed class EscapedToken : Token
    {
        public char EscapedChar { get; }
        public EscapedToken(char escapedChar) { EscapedChar = escapedChar; }
    }

    public sealed class UnderscoreToken : Token
    {
        public int Count { get; }
        public UnderscoreToken(int count) { Count = count; }
    }

    public sealed class BracketOpenToken : Token
    {
    }

    public sealed class LinkTailToken : Token
    {
        public string Url { get; }
        public bool Closed { get; }
        public LinkTailToken(string url, bool closed)
        {
            Url = url;
            Closed = closed;
        }
    }
}

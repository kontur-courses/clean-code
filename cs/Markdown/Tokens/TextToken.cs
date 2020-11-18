using System.Collections.Generic;

namespace Markdown
{
    public class TextToken : IToken
    {
        public TextToken(int length, TokenType type, string text, bool isTerminal, List<IToken> subTokens)
        {
            Length = length;
            Type = type;
            Text = text;
            IsTerminal = isTerminal;
            SubTokens = subTokens;
        }


        public int Length { get; }
        public TokenType Type { get; }
        public string Text { get; protected set; }
        public List<IToken> SubTokens { get; set; }
        public bool IsTerminal { get; }
    }
}